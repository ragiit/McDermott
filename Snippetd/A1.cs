 public async Task ImportExcelFile(InputFileChangeEventArgs e)
 {
     PanelVisible = true;
     foreach (var file in e.GetMultipleFiles(1))
     {
         try
         {
             using MemoryStream ms = new();
             await file.OpenReadStream().CopyToAsync(ms);
             ms.Position = 0;

             ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
             using ExcelPackage package = new(ms);
             ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

             var headerNames = new List<string>() { "Name", "Province", "City" };

             if (Enumerable.Range(1, ws.Dimension.End.Column)
                 .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
             {
                 ToastService.ShowInfo("The header must match with the template.");
                 return;
             }

             var list = new List<DistrictDto>(); 

             var provinceNames = new HashSet<string>();
             var cityNames = new HashSet<string>();

             var list1 = new List<ProvinceDto>();
             var list2 = new List<CityDto>();
              
             for (int row = 2; row <= ws.Dimension.End.Row; row++)
             {
                 var prov = ws.Cells[row, 2].Value?.ToString()?.Trim();
                 var city = ws.Cells[row, 3].Value?.ToString()?.Trim();

                 if (!string.IsNullOrEmpty(prov))
                     provinceNames.Add(prov.ToLower());

                 if (!string.IsNullOrEmpty(city))
                     cityNames.Add(city.ToLower());
             }

             list1 = (await Mediator.Send(new GetProvinceQuery(x => provinceNames.Contains(x.Name.ToLower()), 0, 0))).Item1;
             list2 = (await Mediator.Send(new GetCityQuery(x => cityNames.Contains(x.Name.ToLower()), 0, 0))).Item1;

             for (int row = 2; row <= ws.Dimension.End.Row; row++)
             {
                 bool isValid = true;

                 var name = ws.Cells[row, 1].Value?.ToString()?.Trim();
                 var province = ws.Cells[row, 2].Value?.ToString()?.Trim();
                 var city = ws.Cells[row, 3].Value?.ToString()?.Trim();

                 if (string.IsNullOrWhiteSpace(name))
                 {
                     ToastService.ShowErrorImport(row, 1, name ?? string.Empty);
                     isValid = false;
                 }

                 long? provinceId = null;
                 if (!string.IsNullOrEmpty(province))
                 {
                     var cachedParent = list1.FirstOrDefault(x => x.Name.Equals(province, StringComparison.CurrentCultureIgnoreCase));
                     if (cachedParent is null)
                     {
                         ToastService.ShowErrorImport(row, 2, province ?? string.Empty);
                         isValid = false;
                     }
                     else
                     {
                         provinceId = cachedParent.Id;
                     }
                 }
                 else
                 {
                     ToastService.ShowErrorImport(row, 2, province ?? string.Empty);
                     isValid = false;
                 }

                 long? cityId = null;
                 if (!string.IsNullOrEmpty(city))
                 {
                     var cachedParent = list2.FirstOrDefault(x => x.Name.Equals(city, StringComparison.CurrentCultureIgnoreCase));
                     if (cachedParent is null)
                     {
                         isValid = false;
                         ToastService.ShowErrorImport(row, 3, city ?? string.Empty);
                     }
                     else
                     {
                         cityId = cachedParent.Id;
                     }
                 }
                 else
                 {
                     ToastService.ShowErrorImport(row, 3, city ?? string.Empty);
                     isValid = false;
                 }

                 if (!isValid)
                     continue;

                 var newMenu = new DistrictDto
                 {
                     ProvinceId = provinceId,
                     CityId = cityId,
                     Name = name,
                 };

                 list.Add(newMenu);
             }

             if (list.Count > 0)
             {
                 list = list.DistinctBy(x => new { x.ProvinceId, x.Name, x.CityId }).ToList();

                 // Panggil BulkValidateVillageQuery untuk validasi bulk
                 var existingVillages = await Mediator.Send(new BulkValidateDistrictQuery(list));

                 // Filter village baru yang tidak ada di database
                 list = list.Where(village =>
                     !existingVillages.Any(ev =>
                         ev.Name == village.Name &&
                         ev.ProvinceId == village.ProvinceId &&
                         ev.CityId == village.CityId
                     )
                 ).ToList();

                 await Mediator.Send(new CreateListDistrictRequest(list));
                 await LoadData(0, pageSize);
                 SelectedDataItems = [];
             }

             ToastService.ShowSuccessCountImported(list.Count);
         }
         catch (Exception ex)
         {
             ToastService.ShowError(ex.Message);
         }
         finally { PanelVisible = false; }
     }
     PanelVisible = false;
 }


"DefaultConnection": "Server=.\\ITSSB;Database=A13;MultipleActiveResultSets=true;TrustServerCertificate=True; Trusted_Connection=True;Max Pool Size=10000;"
