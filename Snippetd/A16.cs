Uom

public class UomCommand
 {
     #region GET

    public class GetSingleUomQuery : IRequest<UomDto>
    {
        public List<Expression<Func<Uom, object>>> Includes { get; set; }
        public Expression<Func<Uom, bool>> Predicate { get; set; }
        public Expression<Func<Uom, Uom>> Select { get; set; }

        public List<(Expression<Func<Uom, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetUomQuery : IRequest<(List<UomDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<Uom, object>>> Includes { get; set; }
        public Expression<Func<Uom, bool>> Predicate { get; set; }
        public Expression<Func<Uom, Uom>> Select { get; set; }

        public List<(Expression<Func<Uom, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateUom(Expression<Func<Uom, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<Uom, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateUomRequest(UomDto UomDto) : IRequest<UomDto>
     {
         public UomDto UomDto { get; set; } = UomDto;
     }

     public class BulkValidateUom(List<UomDto> UomsToValidate) : IRequest<List<UomDto>>
     {
         public List<UomDto> UomsToValidate { get; } = UomsToValidate;
     }

     public class CreateListUomRequest(List<UomDto> UomDtos) : IRequest<List<UomDto>>
     {
         public List<UomDto> UomDtos { get; set; } = UomDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateUomRequest(UomDto UomDto) : IRequest<UomDto>
     {
         public UomDto UomDto { get; set; } = UomDto;
     }

     public class UpdateListUomRequest(List<UomDto> UomDtos) : IRequest<List<UomDto>>
     {
         public List<UomDto> UomDtos { get; set; } = UomDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteUomRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateUomQuery, List<UomDto>>,
  
IRequestHandler<GetUomQuery, (List<UomDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleUomQuery, UomDto>,
public class UomHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetUomQuery, (List<UomDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleUomQuery, UomDto>, 
     IRequestHandler<ValidateUom, bool>,
     IRequestHandler<CreateUomRequest, UomDto>,
     IRequestHandler<BulkValidateUom, List<UomDto>>,
     IRequestHandler<CreateListUomRequest, List<UomDto>>,
     IRequestHandler<UpdateUomRequest, UomDto>,
     IRequestHandler<UpdateListUomRequest, List<UomDto>>,
     IRequestHandler<DeleteUomRequest, bool>
{
    #region GET
    public async Task<List<UomDto>> Handle(BulkValidateUom request, CancellationToken cancellationToken)
    {
        var UomDtos = request.UomsToValidate;

        // Ekstrak semua kombinasi yang akan dicari di database
        //var UomNames = UomDtos.Select(x => x.Name).Distinct().ToList();
        //var Codes = UomDtos.Select(x => x.Code).Distinct().ToList();

        //var existingUoms = await _unitOfWork.Repository<Uom>()
        //    .Entities
        //    .AsNoTracking()
        //    .Where(v => UomNames.Contains(v.Name) && Codes.Contains(v.Code))
        //    .ToListAsync(cancellationToken);

        //return existingUoms.Adapt<List<UomDto>>();

        return [];
    }
    public async Task<bool> Handle(ValidateUom request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<Uom>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<UomDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetUomQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Uom>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<Uom>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Uom>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Uom.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Uom
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

                return (pagedItems.Adapt<List<UomDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<UomDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<UomDto> Handle(GetSingleUomQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Uom>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<Uom>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Uom>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.Uom.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Uom
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<UomDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<UomDto> Handle(CreateUomRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Uom>().AddAsync(request.UomDto.Adapt<CreateUpdateUomDto>().Adapt<Uom>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<UomDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<UomDto>> Handle(CreateListUomRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Uom>().AddAsync(request.UomDtos.Adapt<List<Uom>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<UomDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<UomDto> Handle(UpdateUomRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Uom>().UpdateAsync(request.UomDto.Adapt<UomDto>().Adapt<Uom>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<UomDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<UomDto>> Handle(UpdateListUomRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Uom>().UpdateAsync(request.UomDtos.Adapt<List<Uom>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<UomDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteUomRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<Uom>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<Uom>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateUomQuery(List<UomDto> UomsToValidate) : IRequest<List<UomDto>>
{
    public List<UomDto> UomsToValidate { get; } = UomsToValidate;
}a


IRequestHandler<BulkValidateUomQuery, List<UomDto>>,
  
IRequestHandler<GetUomQuery, (List<UomDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleUomQuery, UomDto>,



 var a = await Mediator.Send(new GetUomsQuery
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

var patienss = (await Mediator.Send(new GetSingleUomQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new Uom
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
    var result = await Mediator.Send(new GetUomQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    Uoms = result.Item1;
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

 var result = await Mediator.Send(new GetUomQuery
 {
     Predicate = x => x.UomId == UomId,
     SearchTerm = refUomComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 Uoms = result.Item1;
 totalCountUom = result.PageCount;

 Uoms = (await Mediator.Send(new GetUomQuery
 {
     Predicate = x => x.Id == UomForm.IdCardUomId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleUomsQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new Uom
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new Uom
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new Uom
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


#region ComboboxUom

 private DxComboBox<UomDto, long?> refUomComboBox { get; set; }
 private int UomComboBoxIndex { get; set; } = 0;
 private int totalCountUom = 0;

 private async Task OnSearchUom()
 {
     await LoadDataUom();
 }

 private async Task OnSearchUomIndexIncrement()
 {
     if (UomComboBoxIndex < (totalCountUom - 1))
     {
         UomComboBoxIndex++;
         await LoadDataUom(UomComboBoxIndex, 10);
     }
 }

 private async Task OnSearchUomIndexDecrement()
 {
     if (UomComboBoxIndex > 0)
     {
         UomComboBoxIndex--;
         await LoadDataUom(UomComboBoxIndex, 10);
     }
 }

 private async Task OnInputUomChanged(string e)
 {
     UomComboBoxIndex = 0;
     await LoadDataUom();
 }

 
  private async Task LoadDataUom(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetUomQuery
          {
              SearchTerm = refUomComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Uoms = result.Item1;
          totalCountUom = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxUom

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Uom" ColSpanMd="12">
    <MyDxComboBox Data="@Uoms"
                  NullText="Select Uom"
                  @ref="refUomComboBox"
                  @bind-Value="@a.UomId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputUomChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchUomIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchUom"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchUomIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(UomDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Uom.Name" Caption="Uom" />
            <DxListEditorColumn FieldName="@nameof(UomDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.UomId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<Uom>().AddAsync(request.UomDto.Adapt<CreateUpdateUomDto>().Adapt<Uom>());
var result = await _unitOfWork.Repository<Uom>().AddAsync(request.UomDtos.Adapt<List<CreateUpdateUomDto>>().Adapt<List<Uom>>()); 

var result = await _unitOfWork.Repository<Uom>().UpdateAsync(request.UomDto.Adapt<CreateUpdateUomDto>().Adapt<Uom>());  
var result = await _unitOfWork.Repository<Uom>().UpdateAsync(request.UomDtos.Adapt<List<CreateUpdateUomDto>>().Adapt<List<Uom>>());

list3 = (await Mediator.Send(new GetUomQuery
{
    Predicate = x => UomNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new Uom
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeUomAttendance { get; set; } = 10;
    private int totalCountUomAttendance = 0;
    private int activePageIndexUomAttendance { get; set; } = 0;
    private string searchTermUomAttendance { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedUomAttendance(string searchText)
    {
        searchTermUomAttendance = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeUomAttendance);
    }

    private async Task OnpageSizeUomAttendanceIndexChanged(int newpageSizeUomAttendance)
    {
        pageSizeUomAttendance = newpageSizeUomAttendance;
        await LoadDataOnSearchBoxChanged(0, newpageSizeUomAttendance);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeUomAttendance);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeUomAttendance = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetUomAttendanceQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeUomAttendance,
            SearchTerm = searchTermUomAttendance,
        });
        UomAttendances = result.Item1;
        totalCountUomAttendance = result.PageCount;
        activePageIndexUomAttendance = pageIndex;
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
                 Data="Uoms"
                 @bind-SelectedDataItems="@SelectedDetailDataItems"
                 EditModelSaving="OnSaveInventoryAdjumentDetail"
                 DataItemDeleting="OnDeleteInventoryAdjumentDetail"
                 EditFormButtonsVisible="false"
                 FocusedRowChanged="GridDetail_FocusedRowChanged"
                 SearchTextChanged="OnSearchBoxChanged"
                 KeyFieldName="Id">


     <ToolbarTemplate>
         <MyDxToolbarBase TItem="UomDto"
                          Items="@Uoms"
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
         <DxGridDataColumn FieldName="Uom.Name" Caption="Uom"></DxGridDataColumn>
         <DxGridDataColumn FieldName="TeoriticalQty" Caption="Teoritical Qty" />
         <DxGridDataColumn FieldName="RealQty" Caption="Real Qty" />
         <DxGridDataColumn FieldName="Difference" Caption="Difference" />
         <DxGridDataColumn FieldName="Batch" Caption="Lot Serial Number" />
         <DxGridDataColumn FieldName="ExpiredDate" Caption="Expired Date" SortIndex="0" DisplayFormat="@Helper.DefaultFormatDate" />
         <DxGridDataColumn FieldName="Uom.Uom.Name" Caption="Uom" />
     </Columns>
     <EditFormTemplate Context="EditFormContext">
         @{
             if (EditFormContext.DataItem is null)
             {
                 FormUom = (UomDto)EditFormContext.EditModel;
             }
             var IsBatch = Uoms.FirstOrDefault(x => x.Id == FormUom.UomId)?.TraceAbility ?? false;

             ActiveButton = FormUom.UomId is null ||
             string.IsNullOrWhiteSpace(FormUom.Batch) && IsBatch ||
             FormUom.ExpiredDate is null ||
             FormUom.UomId is null;
         }
         <div class="row w-100">
             <DxFormLayout CssClass="w-100">
                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="Uom" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox Data="@Uoms"
                                     @bind-Value="@FormUom.UomId"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     NullText="Select Uom..."
                                     TextFieldName="Name"
                                     ReadOnly="@(FormUom.Id != 0)"
                                     ValueFieldName="Id"
                                     SelectedItemChanged="@(async (UomDto freq) => await OnSelectUom(freq))"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     ShowValidationIcon="true" />
                         <ValidationMessage For="@(()=> FormUom.UomId)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem Caption="Batch" Enabled="FormUom.Id == 0" Visible="IsBatch" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <MyDxComboBox Data="@Batch"
                                       ReadOnly="@(FormUom.Id != 0)"
                                       NullText="Select Batch..."
                                       AllowUserInput="true"
                                       @bind-Value="@FormUom.Batch"
                                       @bind-Text="@FormUom.Batch"
                                       SelectedItemChanged="@((string a)=> SelectedBatch(a))" />
                         <ValidationMessage For="@(() => FormUom.Batch)" />

                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Teoritical Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     ReadOnly
                                     MinValue="0"
                                     @bind-Value="@FormUom.TeoriticalQty"
                                     NullText="Teoritical Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormUom.TeoriticalQty)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Real Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     MinValue="0"
                                     @bind-Value="@FormUom.RealQty"
                                     NullText="Real Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormUom.RealQty)"   />
                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="Expired Date" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxDateEdit ShowValidationIcon="true"
                                     ReadOnly="@(FormUom.Id != 0)"
                                     DisplayFormat="@Helper.DefaultFormatDate"
                                     @bind-Date="@FormUom.ExpiredDate"
                                     NullText="Expired Date">
                         </DxDateEdit>
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption required-caption" Caption="Uom" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox ShowValidationIcon="true" Data="@Uoms"
                                     NullText="Uom"
                                     ReadOnly="@(FormUom.Id != 0)"
                                     TextFieldName="Name"
                                     ValueFieldName="Id"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     @bind-Value="FormUom.UomId">
                         </DxComboBox>
                         <ValidationMessage For="@(() => FormUom.UomId)" />
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

Uom

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Uom" ColSpanMd="12">
    <DxComboBox Data="Uoms"
                AllowUserInput="true"
                NullText="Select Uom"
                ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputUom"
                @bind-Value="a.UomId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(Uom.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(Uom.Code)" Caption="Code" />
        </Columns>
    </DxComboBox>
    <ValidationMessage For="@(()=>a.UomId)" />
</DxFormLayoutItem>

#region ComboBox Uom
 
private CancellationTokenSource? _ctsUom;
private async Task OnInputUom(ChangeEventArgs e)
{
    try
    {
        PanelVisible = true;
            
        _ctsUom?.Cancel();
        _ctsUom?.Dispose();
        _ctsUom = new CancellationTokenSource();
            
        await Task.Delay(700, _ctsUom.Token);
            
        await LoadUom(e.Value?.ToString() ?? "");
    } 
    finally
    {
        PanelVisible = false;

        // Untuk menghindari kebocoran memori (memory leaks).
        _ctsUom?.Dispose();
        _ctsUom = null;
    } 
}

 private async Task LoadUom(string? e = "", Expression<Func<Uom, bool>>? predicate = null)
 {
     try
     {
         PanelVisible = true;
         Uoms = await Mediator.QueryGetComboBox<Uom, UomDto>(e, predicate);
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
await LoadUom(id:  a.UomId);

Uom



VIRAL 2025

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Uom" ColSpanMd="12">
    <MyDxComboBox Data="Uoms"
                NullText="Select Uom"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputUom"
                SelectedItemChanged="((UomDto e) => SelectedItemChanged(e))"  
                @bind-Value="a.UomId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(Uom.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(Uom.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.UomId)" />
</DxFormLayoutItem>


#region ComboBox Uom

private UomDto SelectedUom { get; set; } = new();
async Task SelectedItemChanged(UomDto e)
{
    if (e is null)
    {
        SelectedUom = new();
        await LoadUom(); 
    }
    else
        SelectedUom = e;
}

private CancellationTokenSource? _ctsUom;
private async Task OnInputUom(ChangeEventArgs e)
{
    try
    {
        PanelVisible = true;

        _cts?.Cancel();
        _ctsUom?.Dispose();
        _ctsUom = new CancellationTokenSource();

        await Task.Delay(Helper.CBX_DELAY, _ctsUom.Token);

        await LoadUom(e.Value?.ToString() ?? "");
    }
    finally
    {
        PanelVisible = false;

        // Untuk menghindari kebocoran memori (memory leaks).
        _ctsUom?.Dispose();
        _ctsUom = null;
    }
}

private async Task LoadUom(string? e = "", Expression<Func<Uom, bool>>? predicate = null)
{
    try
    {
        PanelVisible = true;
        Countries = await Mediator.QueryGetComboBox<Uom, UomDto>(e, predicate);
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}

#endregion