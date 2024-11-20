Company

public class CompanyCommand
 {
     #region GET

    public class GetSingleCompanyQuery : IRequest<CompanyDto>
    {
        public List<Expression<Func<Company, object>>> Includes { get; set; }
        public Expression<Func<Company, bool>> Predicate { get; set; }
        public Expression<Func<Company, Company>> Select { get; set; }

        public List<(Expression<Func<Company, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetCompanyQuery : IRequest<(List<CompanyDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<Company, object>>> Includes { get; set; }
        public Expression<Func<Company, bool>> Predicate { get; set; }
        public Expression<Func<Company, Company>> Select { get; set; }

        public List<(Expression<Func<Company, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateCompany(Expression<Func<Company, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<Company, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateCompanyRequest(CompanyDto CompanyDto) : IRequest<CompanyDto>
     {
         public CompanyDto CompanyDto { get; set; } = CompanyDto;
     }

     public class BulkValidateCompany(List<CompanyDto> CompanysToValidate) : IRequest<List<CompanyDto>>
     {
         public List<CompanyDto> CompanysToValidate { get; } = CompanysToValidate;
     }

     public class CreateListCompanyRequest(List<CompanyDto> CompanyDtos) : IRequest<List<CompanyDto>>
     {
         public List<CompanyDto> CompanyDtos { get; set; } = CompanyDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateCompanyRequest(CompanyDto CompanyDto) : IRequest<CompanyDto>
     {
         public CompanyDto CompanyDto { get; set; } = CompanyDto;
     }

     public class UpdateListCompanyRequest(List<CompanyDto> CompanyDtos) : IRequest<List<CompanyDto>>
     {
         public List<CompanyDto> CompanyDtos { get; set; } = CompanyDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteCompanyRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateCompanyQuery, List<CompanyDto>>,
  
IRequestHandler<GetCompanyQuery, (List<CompanyDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleCompanyQuery, CompanyDto>,
public class CompanyHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetCompanyQuery, (List<CompanyDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleCompanyQuery, CompanyDto>, 
     IRequestHandler<ValidateCompany, bool>,
     IRequestHandler<CreateCompanyRequest, CompanyDto>,
     IRequestHandler<BulkValidateCompany, List<CompanyDto>>,
     IRequestHandler<CreateListCompanyRequest, List<CompanyDto>>,
     IRequestHandler<UpdateCompanyRequest, CompanyDto>,
     IRequestHandler<UpdateListCompanyRequest, List<CompanyDto>>,
     IRequestHandler<DeleteCompanyRequest, bool>
{
    #region GET
    public async Task<List<CompanyDto>> Handle(BulkValidateCompany request, CancellationToken cancellationToken)
    {
        var CompanyDtos = request.CompanysToValidate;

        // Ekstrak semua kombinasi yang akan dicari di database
        //var CompanyNames = CompanyDtos.Select(x => x.Name).Distinct().ToList();
        //var Codes = CompanyDtos.Select(x => x.Code).Distinct().ToList();

        //var existingCompanys = await _unitOfWork.Repository<Company>()
        //    .Entities
        //    .AsNoTracking()
        //    .Where(v => CompanyNames.Contains(v.Name) && Codes.Contains(v.Code))
        //    .ToListAsync(cancellationToken);

        //return existingCompanys.Adapt<List<CompanyDto>>();

        return [];
    }
    public async Task<bool> Handle(ValidateCompany request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<Company>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<CompanyDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Company>().Entities.AsNoTracking(); 

            if (request.Predicate is not null)
                query = query.Where(request.Predicate);

            // Apply ordering
            if (request.OrderByList.Count != 0)
            {
                var firstOrderBy = request.OrderByList.First();
                query = firstOrderBy.IsDescending
                    ? query.OrderByDescending(firstOrderBy.OrderBy)
                    : query.OrderBy(firstOrderBy.OrderBy);

                foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                {
                    query = additionalOrderBy.IsDescending
                        ? ((IOrderedQueryable<Company>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Company>)query).ThenBy(additionalOrderBy.OrderBy);
                }
            }

            // Apply dynamic includes
            if (request.Includes is not null)
            {
                foreach (var includeExpression in request.Includes)
                {
                    query = query.Include(includeExpression);
                }
            }
    
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Company.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Company
                {
                    Id = x.Id, 
                });

            if (!request.IsGetAll)
            { // Paginate and sort
                var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                    query,
                    request.PageSize,
                    request.PageIndex,
                    cancellationToken
                );

                return (pagedItems.Adapt<List<CompanyDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<CompanyDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<CompanyDto> Handle(GetSingleCompanyQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Company>().Entities.AsNoTracking();

            if (request.Predicate is not null)
                query = query.Where(request.Predicate);
            
            // Apply ordering
            if (request.OrderByList.Count != 0)
            {
                var firstOrderBy = request.OrderByList.First();
                query = firstOrderBy.IsDescending
                    ? query.OrderByDescending(firstOrderBy.OrderBy)
                    : query.OrderBy(firstOrderBy.OrderBy);

                foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                {
                    query = additionalOrderBy.IsDescending
                        ? ((IOrderedQueryable<Company>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Company>)query).ThenBy(additionalOrderBy.OrderBy);
                }
            }

            // Apply dynamic includes
            if (request.Includes is not null)
            {
                foreach (var includeExpression in request.Includes)
                {
                    query = query.Include(includeExpression);
                }
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(v =>
                    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    EF.Functions.Like(v.Company.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Company
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<CompanyDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<CompanyDto> Handle(CreateCompanyRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Company>().AddAsync(request.CompanyDto.Adapt<CreateUpdateCompanyDto>().Adapt<Company>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<CompanyDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<CompanyDto>> Handle(CreateListCompanyRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Company>().AddAsync(request.CompanyDtos.Adapt<List<Company>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<CompanyDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<CompanyDto> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Company>().UpdateAsync(request.CompanyDto.Adapt<CompanyDto>().Adapt<Company>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<CompanyDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<CompanyDto>> Handle(UpdateListCompanyRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Company>().UpdateAsync(request.CompanyDtos.Adapt<List<Company>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<CompanyDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<Company>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<Company>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateCompanyQuery(List<CompanyDto> CompanysToValidate) : IRequest<List<CompanyDto>>
{
    public List<CompanyDto> CompanysToValidate { get; } = CompanysToValidate;
}a


IRequestHandler<BulkValidateCompanyQuery, List<CompanyDto>>,
  
IRequestHandler<GetCompanyQuery, (List<CompanyDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleCompanyQuery, CompanyDto>,



 var a = await Mediator.Send(new GetCompanysQuery
 {
     OrderByList =
     [
         (x => x.RegistrationDate, true),               // OrderByDescending RegistrationDate
         (x => x.IsAlertInformationSpecialCase, true),  // ThenByDescending IsAlertInformationSpecialCase
         (x => x.ClassType != null, true)               // ThenByDescending ClassType is not null
     ],
     PageIndex = pageIndex,
     PageSize = pageSize,
 });

var patienss = (await Mediator.Send(new GetSingleCompanyQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new Company
    {
        Id = x.Id,
        IsEmployee = x.IsEmployee,
        Name = x.Name,
        Gender = x.Gender,
        DateOfBirth = x.DateOfBirth
    },
})) ?? new();

try
{
    PanelVisible = true;
    var result = await Mediator.Send(new GetCompanyQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    Companys = result.Item1;
    totalCount = result.PageCount;
    activePageIndex = pageIndex;
}
catch (Exception ex)
{
    ex.HandleException(ToastService);
}
finally
{ 
    PanelVisible = false;
}

 var result = await Mediator.Send(new GetCompanyQuery
 {
     Predicate = x => x.CompanyId == CompanyId,
     SearchTerm = refCompanyComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 Companys = result.Item1;
 totalCountCompany = result.PageCount;

 Companys = (await Mediator.Send(new GetCompanyQuery
 {
     Predicate = x => x.Id == CompanyForm.IdCardCompanyId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleCompanysQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new Company
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new Company
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new Company
        {
            Name = x.Pratitioner.Name,
            SipNo = x.Pratitioner.SipNo
        },
        StartMaternityLeave = x.StartMaternityLeave,
        EndMaternityLeave = x.EndMaternityLeave,
        StartDateSickLeave = x.StartDateSickLeave,
        EndDateSickLeave = x.EndDateSickLeave,
    }
})) ?? new();


#region ComboboxCompany

 private DxComboBox<CompanyDto, long?> refCompanyComboBox { get; set; }
 private int CompanyComboBoxIndex { get; set; } = 0;
 private int totalCountCompany = 0;

 private async Task OnSearchCompany()
 {
     await LoadDataCompany();
 }

 private async Task OnSearchCompanyIndexIncrement()
 {
     if (CompanyComboBoxIndex < (totalCountCompany - 1))
     {
         CompanyComboBoxIndex++;
         await LoadDataCompany(CompanyComboBoxIndex, 10);
     }
 }

 private async Task OnSearchCompanyIndexDecrement()
 {
     if (CompanyComboBoxIndex > 0)
     {
         CompanyComboBoxIndex--;
         await LoadDataCompany(CompanyComboBoxIndex, 10);
     }
 }

 private async Task OnInputCompanyChanged(string e)
 {
     CompanyComboBoxIndex = 0;
     await LoadDataCompany();
 }

 
  private async Task LoadDataCompany(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetCompanyQuery
          {
              SearchTerm = refCompanyComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Companys = result.Item1;
          totalCountCompany = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxCompany

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Company" ColSpanMd="12">
    <MyDxComboBox Data="@Companys"
                  NullText="Select Company"
                  @ref="refCompanyComboBox"
                  @bind-Value="@a.CompanyId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputCompanyChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchCompanyIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchCompany"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchCompanyIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(CompanyDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Company.Name" Caption="Company" />
            <DxListEditorColumn FieldName="@nameof(CompanyDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.CompanyId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<Company>().AddAsync(request.CompanyDto.Adapt<CreateUpdateCompanyDto>().Adapt<Company>());
var result = await _unitOfWork.Repository<Company>().AddAsync(request.CompanyDtos.Adapt<List<CreateUpdateCompanyDto>>().Adapt<List<Company>>()); 

var result = await _unitOfWork.Repository<Company>().UpdateAsync(request.CompanyDto.Adapt<CreateUpdateCompanyDto>().Adapt<Company>());  
var result = await _unitOfWork.Repository<Company>().UpdateAsync(request.CompanyDtos.Adapt<List<CreateUpdateCompanyDto>>().Adapt<List<Company>>());

list3 = (await Mediator.Send(new GetCompanyQuery
{
    Predicate = x => CompanyNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new Company
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeCompanyAttendance { get; set; } = 10;
    private int totalCountCompanyAttendance = 0;
    private int activePageIndexCompanyAttendance { get; set; } = 0;
    private string searchTermCompanyAttendance { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedCompanyAttendance(string searchText)
    {
        searchTermCompanyAttendance = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeCompanyAttendance);
    }

    private async Task OnpageSizeCompanyAttendanceIndexChanged(int newpageSizeCompanyAttendance)
    {
        pageSizeCompanyAttendance = newpageSizeCompanyAttendance;
        await LoadDataOnSearchBoxChanged(0, newpageSizeCompanyAttendance);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeCompanyAttendance);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeCompanyAttendance = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetCompanyAttendanceQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeCompanyAttendance,
            SearchTerm = searchTermCompanyAttendance,
        });
        CompanyAttendances = result.Item1;
        totalCountCompanyAttendance = result.PageCount;
        activePageIndexCompanyAttendance = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}
    #endregion Searching




      #region Searching

  private int pageSize { get; set; } = 10;
  private int totalCount = 0;
  private int activePageIndex { get; set; } = 0;
  private string searchTerm { get; set; } = string.Empty;

  private async Task OnSearchBoxChanged(string searchText)
  {
      searchTerm = searchText;
      await LoadData(0, pageSize);
  }

  private async Task OnPageSizeIndexChanged(int newPageSize)
  {
      pageSize = newPageSize;
      await LoadData(0, newPageSize);
  }

  private async Task OnPageIndexChanged(int newPageIndex)
  {
      await LoadData(newPageIndex, pageSize);
  }

  private async Task LoadData(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          SelectedDataItems = [];
          var a = await Mediator.Send(new GetGeneralConsultanServicesQuery
          {
              OrderByList =
              [
                  (x => x.RegistrationDate, true),               // OrderByDescending RegistrationDate
                  (x => x.IsAlertInformationSpecialCase, true),  // ThenByDescending IsAlertInformationSpecialCase
                  (x => x.ClassType != null, true)               // ThenByDescending ClassType is not null
              ],
              Predicate = x => x.IsVaccination == true,
              PageIndex = pageIndex,
              PageSize = pageSize,
              SearchTerm = searchTerm,
          });

          GeneralConsultanServices = a.Item1;
          totalCount = a.PageCount;
          activePageIndex = pageIndex;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

  #endregion Searching


   <MyGridPaginate @ref="GridDetail"
                 Data="Companys"
                 @bind-SelectedDataItems="@SelectedDetailDataItems"
                 EditModelSaving="OnSaveInventoryAdjumentDetail"
                 DataItemDeleting="OnDeleteInventoryAdjumentDetail"
                 EditFormButtonsVisible="false"
                 FocusedRowChanged="GridDetail_FocusedRowChanged"
                 SearchTextChanged="OnSearchBoxChanged"
                 KeyFieldName="Id">


     <ToolbarTemplate>
         <MyDxToolbarBase TItem="CompanyDto"
                          Items="@Companys"
                          Grid="GridDetail"
                          SelectedDataItems="@SelectedDetailDataItems"
                          NewItem_Click="@NewItem_Click"
                          EditItem_Click="@EditItem_Click"
                          DeleteItem_Click="@DeleteItem_Click"
                          Refresh_Click="@(async () => await LoadData())"
                          IsImport="UserAccessCRUID.IsImport"
                          VisibleNew="UserAccessCRUID.IsCreate"
                          VisibleEdit="UserAccessCRUID.IsUpdate"
                          VisibleDelete="UserAccessCRUID.IsDelete" />
     </ToolbarTemplate>


     <Columns>
         <DxGridSelectionColumn Width="15px" />
         <DxGridDataColumn FieldName="Company.Name" Caption="Company"></DxGridDataColumn>
         <DxGridDataColumn FieldName="TeoriticalQty" Caption="Teoritical Qty" />
         <DxGridDataColumn FieldName="RealQty" Caption="Real Qty" />
         <DxGridDataColumn FieldName="Difference" Caption="Difference" />
         <DxGridDataColumn FieldName="Batch" Caption="Lot Serial Number" />
         <DxGridDataColumn FieldName="ExpiredDate" Caption="Expired Date" SortIndex="0" DisplayFormat="@Helper.DefaultFormatDate" />
         <DxGridDataColumn FieldName="Company.Company.Name" Caption="Company" />
     </Columns>
     <EditFormTemplate Context="EditFormContext">
         @{
             if (EditFormContext.DataItem is null)
             {
                 FormCompany = (CompanyDto)EditFormContext.EditModel;
             }
             var IsBatch = Companys.FirstOrDefault(x => x.Id == FormCompany.CompanyId)?.TraceAbility ?? false;

             ActiveButton = FormCompany.CompanyId is null ||
             string.IsNullOrWhiteSpace(FormCompany.Batch) && IsBatch ||
             FormCompany.ExpiredDate is null ||
             FormCompany.CompanyId is null;
         }
         <div class="row w-100">
             <DxFormLayout CssClass="w-100">
                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="Company" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox Data="@Companys"
                                     @bind-Value="@FormCompany.CompanyId"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     NullText="Select Company..."
                                     TextFieldName="Name"
                                     ReadOnly="@(FormCompany.Id != 0)"
                                     ValueFieldName="Id"
                                     SelectedItemChanged="@(async (CompanyDto freq) => await OnSelectCompany(freq))"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     ShowValidationIcon="true" />
                         <ValidationMessage For="@(()=> FormCompany.CompanyId)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem Caption="Batch" Enabled="FormCompany.Id == 0" Visible="IsBatch" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <MyDxComboBox Data="@Batch"
                                       ReadOnly="@(FormCompany.Id != 0)"
                                       NullText="Select Batch..."
                                       AllowUserInput="true"
                                       @bind-Value="@FormCompany.Batch"
                                       @bind-Text="@FormCompany.Batch"
                                       SelectedItemChanged="@((string a)=> SelectedBatch(a))" />
                         <ValidationMessage For="@(() => FormCompany.Batch)" />

                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Teoritical Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     ReadOnly
                                     MinValue="0"
                                     @bind-Value="@FormCompany.TeoriticalQty"
                                     NullText="Teoritical Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormCompany.TeoriticalQty)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Real Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     MinValue="0"
                                     @bind-Value="@FormCompany.RealQty"
                                     NullText="Real Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormCompany.RealQty)"   />
                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="Expired Date" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxDateEdit ShowValidationIcon="true"
                                     ReadOnly="@(FormCompany.Id != 0)"
                                     DisplayFormat="@Helper.DefaultFormatDate"
                                     @bind-Date="@FormCompany.ExpiredDate"
                                     NullText="Expired Date">
                         </DxDateEdit>
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption required-caption" Caption="Company" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox ShowValidationIcon="true" Data="@Companys"
                                     NullText="Company"
                                     ReadOnly="@(FormCompany.Id != 0)"
                                     TextFieldName="Name"
                                     ValueFieldName="Id"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     @bind-Value="FormCompany.CompanyId">
                         </DxComboBox>
                         <ValidationMessage For="@(() => FormCompany.CompanyId)" />
                     </DxFormLayoutItem>
                 </div>
             </DxFormLayout>

             <div class="col-md-12 d-flex justify-content-end">
                 <DxButton Enabled="@(!ActiveButton)" RenderStyle="ButtonRenderStyle.Primary" RenderStyleMode="@ButtonRenderStyleMode.Contained" IconCssClass="fa-solid fa-sd-card" Text="Save" SubmitFormOnClick="true"></DxButton>
                 <DxButton RenderStyle="ButtonRenderStyle.Danger" RenderStyleMode="@ButtonRenderStyleMode.Contained" IconCssClass="fa-solid fa-xmark" Text="Cancel" Click="@(() => GridDetail.CancelEditAsync())"></DxButton>
             </div>

         </div>
     </EditFormTemplate>
 </MyGridPaginate>




 NEW COMBOBOX VIRAL 2024

Company

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Company" ColSpanMd="12">
    <DxComboBox Data="Companys"
                AllowUserInput="true"
                NullText="Select Company"
                ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputCompany"
                @bind-Value="a.CompanyId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(Company.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(Company.Code)" Caption="Code" />
        </Columns>
    </DxComboBox>
    <ValidationMessage For="@(()=>a.CompanyId)" />
</DxFormLayoutItem>

#region ComboBox Company
 
private CancellationTokenSource? _ctsCompany;
private async Task OnInputCompany(ChangeEventArgs e)
{
    try
    {
        PanelVisible = true;
            
        _ctsCompany?.Cancel();
        _ctsCompany?.Dispose();
        _ctsCompany = new CancellationTokenSource();
            
        await Task.Delay(700, _ctsCompany.Token);
            
        await LoadCompany(e.Value?.ToString() ?? "");
    } 
    finally
    {
        PanelVisible = false;

        // Untuk menghindari kebocoran memori (memory leaks).
        _ctsCompany?.Dispose();
        _ctsCompany = null;
    } 
}

 private async Task LoadCompany(string? e = "", Expression<Func<Company, bool>>? predicate = null)
 {
     try
     {
         PanelVisible = true;
         Companys = await Mediator.QueryGetComboBox<Company, CompanyDto>(e, predicate);
         PanelVisible = false;
     }
     catch (Exception ex)
     {
         ex.HandleException(ToastService);
     }
     finally { PanelVisible = false; }
 }

#endregion


// Ini buat di EditItemClick
await LoadCompany(id:  a.CompanyId);

Company



VIRAL 2025

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Province" ColSpanMd="12">
    <MyDxComboBox Data="Countries"
                NullText="Select Province"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputProvince"
                SelectedItemChanged="((ProvinceDto e) => SelectedItemChanged(e))"  
                @bind-Value="a.ProvinceId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(Province.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(Province.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.ProvinceId)" />
</DxFormLayoutItem>


#region ComboBox Province

private ProvinceDto SelectedProvince { get; set; } = new();
async Task SelectedItemChanged(ProvinceDto e)
{
    if (e is null)
    {
        SelectedProvince = new();
        await LoadCounty(); 
    }
    else
        SelectedProvince = e;
}

private CancellationTokenSource? _cts;
private async Task OnInputProvince(ChangeEventArgs e)
{
    try
    {
        PanelVisible = true;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        await Task.Delay(Helper.CBX_DELAY, _cts.Token);

        await LoadCounty(e.Value?.ToString() ?? "");
    }
    finally
    {
        PanelVisible = false;

        // Untuk menghindari kebocoran memori (memory leaks).
        _cts?.Dispose();
        _cts = null;
    }
}

private async Task LoadCounty(string? e = "", Expression<Func<Province, bool>>? predicate = null)
{
    try
    {
        PanelVisible = true;
        Countries = await Mediator.QueryGetComboBox<Province, ProvinceDto>(e, predicate);
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}

#endregion