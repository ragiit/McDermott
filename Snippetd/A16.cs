OTPRequest

public class OTPRequestCommand
 {
     #region GET

    public class GetSingleOTPRequestQuery : IRequest<OTPRequestDto>
    {
        public List<Expression<Func<OTPRequest, object>>> Includes { get; set; }
        public Expression<Func<OTPRequest, bool>> Predicate { get; set; }
        public Expression<Func<OTPRequest, OTPRequest>> Select { get; set; }

        public List<(Expression<Func<OTPRequest, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetOTPRequestQuery : IRequest<(List<OTPRequestDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<OTPRequest, object>>> Includes { get; set; }
        public Expression<Func<OTPRequest, bool>> Predicate { get; set; }
        public Expression<Func<OTPRequest, OTPRequest>> Select { get; set; }

        public List<(Expression<Func<OTPRequest, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateOTPRequest(Expression<Func<OTPRequest, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<OTPRequest, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateOTPRequestRequest(OTPRequestDto OTPRequestDto) : IRequest<OTPRequestDto>
     {
         public OTPRequestDto OTPRequestDto { get; set; } = OTPRequestDto;
     }

     public class BulkValidateOTPRequest(List<OTPRequestDto> OTPRequestsToValidate) : IRequest<List<OTPRequestDto>>
     {
         public List<OTPRequestDto> OTPRequestsToValidate { get; } = OTPRequestsToValidate;
     }

     public class CreateListOTPRequestRequest(List<OTPRequestDto> OTPRequestDtos) : IRequest<List<OTPRequestDto>>
     {
         public List<OTPRequestDto> OTPRequestDtos { get; set; } = OTPRequestDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateOTPRequestRequest(OTPRequestDto OTPRequestDto) : IRequest<OTPRequestDto>
     {
         public OTPRequestDto OTPRequestDto { get; set; } = OTPRequestDto;
     }

     public class UpdateListOTPRequestRequest(List<OTPRequestDto> OTPRequestDtos) : IRequest<List<OTPRequestDto>>
     {
         public List<OTPRequestDto> OTPRequestDtos { get; set; } = OTPRequestDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteOTPRequestRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateOTPRequestQuery, List<OTPRequestDto>>,
  
IRequestHandler<GetOTPRequestQuery, (List<OTPRequestDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleOTPRequestQuery, OTPRequestDto>,
public class OTPRequestHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetOTPRequestQuery, (List<OTPRequestDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleOTPRequestQuery, OTPRequestDto>, 
     IRequestHandler<ValidateOTPRequest, bool>,
     IRequestHandler<CreateOTPRequestRequest, OTPRequestDto>,
     IRequestHandler<BulkValidateOTPRequest, List<OTPRequestDto>>,
     IRequestHandler<CreateListOTPRequestRequest, List<OTPRequestDto>>,
     IRequestHandler<UpdateOTPRequestRequest, OTPRequestDto>,
     IRequestHandler<UpdateListOTPRequestRequest, List<OTPRequestDto>>,
     IRequestHandler<DeleteOTPRequestRequest, bool>
{
    #region GET
    public async Task<List<OTPRequestDto>> Handle(BulkValidateOTPRequest request, CancellationToken cancellationToken)
    {
        var OTPRequestDtos = request.OTPRequestsToValidate;

        // Ekstrak semua kombinasi yang akan dicari di database
        //var OTPRequestNames = OTPRequestDtos.Select(x => x.Name).Distinct().ToList();
        //var Codes = OTPRequestDtos.Select(x => x.Code).Distinct().ToList();

        //var existingOTPRequests = await _unitOfWork.Repository<OTPRequest>()
        //    .Entities
        //    .AsNoTracking()
        //    .Where(v => OTPRequestNames.Contains(v.Name) && Codes.Contains(v.Code))
        //    .ToListAsync(cancellationToken);

        //return existingOTPRequests.Adapt<List<OTPRequestDto>>();

        return [];
    }
    public async Task<bool> Handle(ValidateOTPRequest request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<OTPRequest>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<OTPRequestDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetOTPRequestQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<OTPRequest>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<OTPRequest>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<OTPRequest>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.OTPRequest.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new OTPRequest
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

                return (pagedItems.Adapt<List<OTPRequestDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<OTPRequestDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<OTPRequestDto> Handle(GetSingleOTPRequestQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<OTPRequest>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<OTPRequest>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<OTPRequest>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.OTPRequest.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new OTPRequest
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<OTPRequestDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<OTPRequestDto> Handle(CreateOTPRequestRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<OTPRequest>().AddAsync(request.OTPRequestDto.Adapt<CreateUpdateOTPRequestDto>().Adapt<OTPRequest>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetOTPRequestQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<OTPRequestDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<OTPRequestDto>> Handle(CreateListOTPRequestRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<OTPRequest>().AddAsync(request.OTPRequestDtos.Adapt<List<OTPRequest>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetOTPRequestQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<OTPRequestDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<OTPRequestDto> Handle(UpdateOTPRequestRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<OTPRequest>().UpdateAsync(request.OTPRequestDto.Adapt<OTPRequestDto>().Adapt<OTPRequest>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetOTPRequestQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<OTPRequestDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<OTPRequestDto>> Handle(UpdateListOTPRequestRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<OTPRequest>().UpdateAsync(request.OTPRequestDtos.Adapt<List<OTPRequest>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetOTPRequestQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<OTPRequestDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteOTPRequestRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<OTPRequest>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<OTPRequest>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetOTPRequestQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateOTPRequestQuery(List<OTPRequestDto> OTPRequestsToValidate) : IRequest<List<OTPRequestDto>>
{
    public List<OTPRequestDto> OTPRequestsToValidate { get; } = OTPRequestsToValidate;
}a


IRequestHandler<BulkValidateOTPRequestQuery, List<OTPRequestDto>>,
  
IRequestHandler<GetOTPRequestQuery, (List<OTPRequestDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleOTPRequestQuery, OTPRequestDto>,



 var a = await Mediator.Send(new GetOTPRequestsQuery
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

var patienss = (await Mediator.Send(new GetSingleOTPRequestQuery
{
    Predicate = x => x.Id == data.DiagnosisBPJSIntegrationTempId,
    Select = x => new OTPRequest
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
    var result = await Mediator.Send(new GetOTPRequestQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    OTPRequests = result.Item1;
    totalCount = result.PageCount;
    activePageIndex = pageIndex;
}
catch (Exception ex)
{
    ex.HandleException(ToastOTPRequest);
}
finally
{ 
    PanelVisible = false;
}

 var result = await Mediator.Send(new GetOTPRequestQuery
 {
     Predicate = x => x.OTPRequestId == OTPRequestId,
     SearchTerm = refOTPRequestComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 OTPRequests = result.Item1;
 totalCountOTPRequest = result.PageCount;

 OTPRequests = (await Mediator.Send(new GetOTPRequestQuery
 {
     Predicate = x => x.Id == OTPRequestForm.IdCardOTPRequestId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleOTPRequestsQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.DiagnosisBPJSIntegrationTemp
    ],
    Select = x => new OTPRequest
    {
        Id = x.Id,
        DiagnosisBPJSIntegrationTempId = x.DiagnosisBPJSIntegrationTempId,
        DiagnosisBPJSIntegrationTemp = new OTPRequest
        {
            DateOfBirth = x.DiagnosisBPJSIntegrationTemp.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new OTPRequest
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


#region ComboboxOTPRequest

 private DxComboBox<OTPRequestDto, long?> refOTPRequestComboBox { get; set; }
 private int OTPRequestComboBoxIndex { get; set; } = 0;
 private int totalCountOTPRequest = 0;

 private async Task OnSearchOTPRequest()
 {
     await LoadDataOTPRequest();
 }

 private async Task OnSearchOTPRequestIndexIncrement()
 {
     if (OTPRequestComboBoxIndex < (totalCountOTPRequest - 1))
     {
         OTPRequestComboBoxIndex++;
         await LoadDataOTPRequest(OTPRequestComboBoxIndex, 10);
     }
 }

 private async Task OnSearchOTPRequestIndexDecrement()
 {
     if (OTPRequestComboBoxIndex > 0)
     {
         OTPRequestComboBoxIndex--;
         await LoadDataOTPRequest(OTPRequestComboBoxIndex, 10);
     }
 }

 private async Task OnInputOTPRequestChanged(string e)
 {
     OTPRequestComboBoxIndex = 0;
     await LoadDataOTPRequest();
 }

 
  private async Task LoadDataOTPRequest(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetOTPRequestQuery
          {
              SearchTerm = refOTPRequestComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          OTPRequests = result.Item1;
          totalCountOTPRequest = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastOTPRequest);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxOTPRequest

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="OTPRequest" ColSpanMd="12">
    <MyDxComboBox Data="@OTPRequests"
                  NullText="Select OTPRequest"
                  @ref="refOTPRequestComboBox"
                  @bind-Value="@a.OTPRequestId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputOTPRequestChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchOTPRequestIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchOTPRequest"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchOTPRequestIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(OTPRequestDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="OTPRequest.Name" Caption="OTPRequest" />
            <DxListEditorColumn FieldName="@nameof(OTPRequestDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.OTPRequestId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<OTPRequest>().AddAsync(request.OTPRequestDto.Adapt<CreateUpdateOTPRequestDto>().Adapt<OTPRequest>());
var result = await _unitOfWork.Repository<OTPRequest>().AddAsync(request.OTPRequestDtos.Adapt<List<CreateUpdateOTPRequestDto>>().Adapt<List<OTPRequest>>()); 

var result = await _unitOfWork.Repository<OTPRequest>().UpdateAsync(request.OTPRequestDto.Adapt<CreateUpdateOTPRequestDto>().Adapt<OTPRequest>());  
var result = await _unitOfWork.Repository<OTPRequest>().UpdateAsync(request.OTPRequestDtos.Adapt<List<CreateUpdateOTPRequestDto>>().Adapt<List<OTPRequest>>());

list3 = (await Mediator.Send(new GetOTPRequestQuery
{
    Predicate = x => OTPRequestNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new OTPRequest
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeOTPRequest { get; set; } = 10;
    private int totalCountOTPRequest = 0;
    private int activePageIndexOTPRequest { get; set; } = 0;
    private string searchTermOTPRequest { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedOTPRequest(string searchText)
    {
        searchTermOTPRequest = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeOTPRequest);
    }

    private async Task OnpageSizeOTPRequestIndexChanged(int newpageSizeOTPRequest)
    {
        pageSizeOTPRequest = newpageSizeOTPRequest;
        await LoadDataOnSearchBoxChanged(0, newpageSizeOTPRequest);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeOTPRequest);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeOTPRequest = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetOTPRequestQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeOTPRequest,
            SearchTerm = searchTermOTPRequest,
        });
        OTPRequests = result.Item1;
        totalCountOTPRequest = result.PageCount;
        activePageIndexOTPRequest = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastOTPRequest);
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
          var a = await Mediator.Send(new GetGeneralConsultanOTPRequestsQuery
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

          GeneralConsultanOTPRequests = a.Item1;
          totalCount = a.PageCount;
          activePageIndex = pageIndex;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastOTPRequest);
      }
      finally { PanelVisible = false; }
  }

  #endregion Searching


   <MyGridPaginate @ref="GridDetail"
                 Data="OTPRequests"
                 @bind-SelectedDataItems="@SelectedDetailDataItems"
                 EditModelSaving="OnSaveInventoryAdjumentDetail"
                 DataItemDeleting="OnDeleteInventoryAdjumentDetail"
                 EditFormButtonsVisible="false"
                 FocusedRowChanged="GridDetail_FocusedRowChanged"
                 SearchTextChanged="OnSearchBoxChanged"
                 KeyFieldName="Id">


     <ToolbarTemplate>
         <MyDxToolbarBase TItem="OTPRequestDto"
                          Items="@OTPRequests"
                          Grid="GridDetail"
                          SelectedDataItems="@SelectedDetailDataItems"
                          NewItem_Click="@NewItem_Click"
                          EditItem_Click="@EditItem_Click"
                          DeleteItem_Click="@DeleteItem_Click"
                          Refresh_Click="@(async () => await LoadData())"
                          IsImport="OTPRequestAccessCRUID.IsImport"
                          VisibleNew="OTPRequestAccessCRUID.IsCreate"
                          VisibleEdit="OTPRequestAccessCRUID.IsUpdate"
                          VisibleDelete="OTPRequestAccessCRUID.IsDelete" />
     </ToolbarTemplate>


     <Columns>
         <DxGridSelectionColumn Width="15px" />
         <DxGridDataColumn FieldName="OTPRequest.Name" Caption="OTPRequest"></DxGridDataColumn>
         <DxGridDataColumn FieldName="TeoriticalQty" Caption="Teoritical Qty" />
         <DxGridDataColumn FieldName="RealQty" Caption="Real Qty" />
         <DxGridDataColumn FieldName="Difference" Caption="Difference" />
         <DxGridDataColumn FieldName="Batch" Caption="Lot Serial Number" />
         <DxGridDataColumn FieldName="ExpiredDate" Caption="Expired Date" SortIndex="0" DisplayFormat="@Helper.DefaultFormatDate" />
         <DxGridDataColumn FieldName="OTPRequest.OTPRequest.Name" Caption="OTPRequest" />
     </Columns>
     <EditFormTemplate Context="EditFormContext">
         @{
             if (EditFormContext.DataItem is null)
             {
                 FormOTPRequest = (OTPRequestDto)EditFormContext.EditModel;
             }
             var IsBatch = OTPRequests.FirstOrDefault(x => x.Id == FormOTPRequest.OTPRequestId)?.TraceAbility ?? false;

             ActiveButton = FormOTPRequest.OTPRequestId is null ||
             string.IsNullOrWhiteSpace(FormOTPRequest.Batch) && IsBatch ||
             FormOTPRequest.ExpiredDate is null ||
             FormOTPRequest.OTPRequestId is null;
         }
         <div class="row w-100">
             <DxFormLayout CssClass="w-100">
                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="OTPRequest" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox Data="@OTPRequests"
                                     @bind-Value="@FormOTPRequest.OTPRequestId"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     NullText="Select OTPRequest..."
                                     TextFieldName="Name"
                                     ReadOnly="@(FormOTPRequest.Id != 0)"
                                     ValueFieldName="Id"
                                     SelectedItemChanged="@(async (OTPRequestDto freq) => await OnSelectOTPRequest(freq))"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     ShowValidationIcon="true" />
                         <ValidationMessage For="@(()=> FormOTPRequest.OTPRequestId)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem Caption="Batch" Enabled="FormOTPRequest.Id == 0" Visible="IsBatch" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <MyDxComboBox Data="@Batch"
                                       ReadOnly="@(FormOTPRequest.Id != 0)"
                                       NullText="Select Batch..."
                                       AllowOTPRequestInput="true"
                                       @bind-Value="@FormOTPRequest.Batch"
                                       @bind-Text="@FormOTPRequest.Batch"
                                       SelectedItemChanged="@((string a)=> SelectedBatch(a))" />
                         <ValidationMessage For="@(() => FormOTPRequest.Batch)" />

                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Teoritical Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     ReadOnly
                                     MinValue="0"
                                     @bind-Value="@FormOTPRequest.TeoriticalQty"
                                     NullText="Teoritical Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormOTPRequest.TeoriticalQty)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Real Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     MinValue="0"
                                     @bind-Value="@FormOTPRequest.RealQty"
                                     NullText="Real Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormOTPRequest.RealQty)"   />
                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="Expired Date" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxDateEdit ShowValidationIcon="true"
                                     ReadOnly="@(FormOTPRequest.Id != 0)"
                                     DisplayFormat="@Helper.DefaultFormatDate"
                                     @bind-Date="@FormOTPRequest.ExpiredDate"
                                     NullText="Expired Date">
                         </DxDateEdit>
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption required-caption" Caption="OTPRequest" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox ShowValidationIcon="true" Data="@OTPRequests"
                                     NullText="OTPRequest"
                                     ReadOnly="@(FormOTPRequest.Id != 0)"
                                     TextFieldName="Name"
                                     ValueFieldName="Id"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     @bind-Value="FormOTPRequest.OTPRequestId">
                         </DxComboBox>
                         <ValidationMessage For="@(() => FormOTPRequest.OTPRequestId)" />
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

OTPRequest

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="OTPRequest" ColSpanMd="12">
    <DxComboBox Data="OTPRequests"
                AllowOTPRequestInput="true"
                NullText="Select OTPRequest"
                ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputOTPRequest"
                @bind-Value="a.OTPRequestId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(OTPRequest.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(OTPRequest.Code)" Caption="Code" />
        </Columns>
    </DxComboBox>
    <ValidationMessage For="@(()=>a.OTPRequestId)" />
</DxFormLayoutItem>

#region ComboBox OTPRequest
 
private CancellationTokenSource? _ctsOTPRequest;
private async Task OnInputOTPRequest(ChangeEventArgs e)
{
    try
    {
        PanelVisible = true;
            
        _ctsOTPRequest?.Cancel();
        _ctsOTPRequest?.Dispose();
        _ctsOTPRequest = new CancellationTokenSource();
            
        await Task.Delay(700, _ctsOTPRequest.Token);
            
        await LoadOTPRequest(e.Value?.ToString() ?? "");
    } 
    finally
    {
        PanelVisible = false;

        // Untuk menghindari kebocoran memori (memory leaks).
        _ctsOTPRequest?.Dispose();
        _ctsOTPRequest = null;
    } 
}

 private async Task LoadOTPRequest(string? e = "", Expression<Func<OTPRequest, bool>>? predicate = null)
 {
     try
     {
         PanelVisible = true;
         OTPRequests = await Mediator.QueryGetComboBox<OTPRequest, OTPRequestDto>(e, predicate);
         PanelVisible = false;
     }
     catch (Exception ex)
     {
         ex.HandleException(ToastOTPRequest);
     }
     finally { PanelVisible = false; }
 }

#endregion


// Ini buat di EditItemClick
await LoadOTPRequest(id:  a.OTPRequestId);

OTPRequest



VIRAL 2025

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="OTPRequest" ColSpanMd="12">
    <MyDxComboBox Data="OTPRequests"
                NullText="Select OTPRequest"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputOTPRequest"
                ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                SelectedItemChanged="((OTPRequestDto e) => SelectedItemChanged(e))"  
                @bind-Value="a.OTPRequestId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(OTPRequest.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(OTPRequest.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.OTPRequestId)" />
</DxFormLayoutItem>


#region ComboBox OTPRequest

    private OTPRequestDto SelectedOTPRequest { get; set; } = new();
    async Task SelectedItemChanged(OTPRequestDto e)
    {
        if (e is null)
        {
            SelectedOTPRequest = new();
            await LoadOTPRequest(); 
        }
        else
            SelectedOTPRequest = e;
    }

    private CancellationTokenSource? _ctsOTPRequest;
    private async Task OnInputOTPRequest(ChangeEventArgs e)
    {
        try
        { 
            _ctsOTPRequest?.Cancel();
            _ctsOTPRequest?.Dispose();
            _ctsOTPRequest = new CancellationTokenSource();

            await Task.Delay(Helper.CBX_DELAY, _ctsOTPRequest.Token);

            await LoadOTPRequest(e.Value?.ToString() ?? "");
        }
        finally
        { 
            _ctsOTPRequest?.Dispose();
            _ctsOTPRequest = null;
        }
    }

    private async Task LoadOTPRequest(string? e = "", Expression<Func<OTPRequest, bool>>? predicate = null)
    {
        try
        { 
            OTPRequests = await Mediator.QueryGetComboBox<OTPRequest, OTPRequestDto>(e, predicate); 
        }
        catch (Exception ex)
        {
            ex.HandleException(ToastOTPRequest);
        }
        finally { PanelVisible = false; }
    }

#endregion

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="DiagnosisBPJSIntegrationTemp" ColSpanMd="12">
     <MyDxComboBox Data="@DiagnosisBPJSIntegrationTemps"
                   NullText="Select DiagnosisBPJSIntegrationTemp"
                   @ref="refDiagnosisBPJSIntegrationTempComboBox"
                   @bind-Value="@GeneralConsultanService.DiagnosisBPJSIntegrationTempId"
                   TextFieldName="Name"
                   ValueFieldName="Id"
                   ReadOnly="@(!GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Planned))"
                   SelectedItemChanged="@((UserDto e) => SelectedItemDiagnosisBPJSIntegrationTempChanged(e))"
                   TextChanged="((string e) => OnInputDiagnosisBPJSIntegrationTempChanged(e))">
         <Buttons>
             <DxEditorButton Click="OnSearchDiagnosisBPJSIntegrationTempIndexDecrement"
                             IconCssClass="fa-solid fa-caret-left"
                             Visible="!ReadOnlyForm() && IsStatus(EnumStatusGeneralConsultantService.Planned)"
                             Tooltip="Previous Index" />
             <DxEditorButton Click="OnSearchDiagnosisBPJSIntegrationTemp"
                             IconCssClass="fa-solid fa-magnifying-glass"
                             Visible="!ReadOnlyForm() && IsStatus(EnumStatusGeneralConsultantService.Planned)"
                             Tooltip="Search" />
             <DxEditorButton Click="OnSearchDiagnosisBPJSIntegrationTempIndexIncrement"
                             Visible="!ReadOnlyForm() && IsStatus(EnumStatusGeneralConsultantService.Planned)"
                             IconCssClass="fa-solid fa-caret-right"
                             Tooltip="Next Index" />
         </Buttons>
         <Columns>
             <DxListEditorColumn FieldName="NoRm" Caption="Medical Record" />
             <DxListEditorColumn FieldName="Name" />
             <DxListEditorColumn FieldName="Email" />
             <DxListEditorColumn FieldName="MobilePhone" Caption="Mobile Phone" />
             <DxListEditorColumn FieldName="Gender" />
             <DxListEditorColumn FieldName="DateOfBirth" Caption="Date Of Birth" />
         </Columns>
     </MyDxComboBox>
     <ValidationMessage For="@(()=>GeneralConsultanService.DiagnosisBPJSIntegrationTempId)" />
 </DxFormLayoutItem>

 #region ComboboxDiagnosisBPJSIntegrationTemp

private DxComboBox<UserDto, long?> refDiagnosisBPJSIntegrationTempComboBox { get; set; }
private int DiagnosisBPJSIntegrationTempComboBoxIndex { get; set; } = 0;
private int totalCountDiagnosisBPJSIntegrationTemp = 0;

private async Task OnSearchDiagnosisBPJSIntegrationTemp()
{
    await LoadDataDiagnosisBPJSIntegrationTemp();
}

private async Task OnSearchDiagnosisBPJSIntegrationTempIndexIncrement()
{
    if (DiagnosisBPJSIntegrationTempComboBoxIndex < (totalCountDiagnosisBPJSIntegrationTemp - 1))
    {
        DiagnosisBPJSIntegrationTempComboBoxIndex++;
        await LoadDataDiagnosisBPJSIntegrationTemp(DiagnosisBPJSIntegrationTempComboBoxIndex, 10);
    }
}

private async Task OnSearchDiagnosisBPJSIntegrationTempIndexDecrement()
{
    if (DiagnosisBPJSIntegrationTempComboBoxIndex > 0)
    {
        DiagnosisBPJSIntegrationTempComboBoxIndex--;
        await LoadDataDiagnosisBPJSIntegrationTemp(DiagnosisBPJSIntegrationTempComboBoxIndex, 10);
    }
}

private async Task OnInputDiagnosisBPJSIntegrationTempChanged(string e)
{
    DiagnosisBPJSIntegrationTempComboBoxIndex = 0;
    await LoadDataDiagnosisBPJSIntegrationTemp();
}

private async Task LoadDataDiagnosisBPJSIntegrationTemp(int pageIndex = 0, int pageSize = 10)
{
    try
    {
        PanelVisible = true;
        var result = await Mediator.Send(new GetUserQueryNew
        {
            Predicate = x => x.IsDiagnosisBPJSIntegrationTemp == true,
            SearchTerm = refDiagnosisBPJSIntegrationTempComboBox?.Text ?? "",
            PageIndex = pageIndex,
            PageSize = pageSize,
            Select = x => new User
            {
                Id = x.Id,
                Name = x.Name,
                NoRm = x.NoRm,
                Email = x.Email,
                MobilePhone = x.MobilePhone,
                Gender = x.Gender,
                DateOfBirth = x.DateOfBirth,
                NoId = x.NoId,
                CurrentMobile = x.CurrentMobile,

                IsWeatherDiagnosisBPJSIntegrationTempAllergyIds = x.IsWeatherDiagnosisBPJSIntegrationTempAllergyIds,
                IsFoodDiagnosisBPJSIntegrationTempAllergyIds = x.IsFoodDiagnosisBPJSIntegrationTempAllergyIds,
                IsPharmacologyDiagnosisBPJSIntegrationTempAllergyIds = x.IsPharmacologyDiagnosisBPJSIntegrationTempAllergyIds,
                WeatherDiagnosisBPJSIntegrationTempAllergyIds = x.WeatherDiagnosisBPJSIntegrationTempAllergyIds,
                FoodDiagnosisBPJSIntegrationTempAllergyIds = x.FoodDiagnosisBPJSIntegrationTempAllergyIds,
                PharmacologyDiagnosisBPJSIntegrationTempAllergyIds = x.PharmacologyDiagnosisBPJSIntegrationTempAllergyIds,

                IsFamilyMedicalHistory = x.IsFamilyMedicalHistory,
                FamilyMedicalHistory = x.FamilyMedicalHistory,
                FamilyMedicalHistoryOther = x.FamilyMedicalHistoryOther,

                IsMedicationHistory = x.IsMedicationHistory,
                MedicationHistory = x.MedicationHistory,
                PastMedicalHistory = x.PastMedicalHistory
            }
        });
        DiagnosisBPJSIntegrationTemps = result.Item1;
        totalCountDiagnosisBPJSIntegrationTemp = result.PageCount;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}

private async Task SelectedItemDiagnosisBPJSIntegrationTempChanged(UserDto e)
{
    GeneralConsultanService.InsurancePolicyId = null;
    InsurancePolicies.Clear();
    GeneralConsultanService.DiagnosisBPJSIntegrationTemp = new();

    if (e is null)
        return;

    GeneralConsultanService.DiagnosisBPJSIntegrationTemp = DiagnosisBPJSIntegrationTemps.FirstOrDefault(x => x.Id == e.Id) ?? new();
    GeneralConsultanService.DiagnosisBPJSIntegrationTempId = e.Id;
    UserForm = DiagnosisBPJSIntegrationTemps.FirstOrDefault(x => x.Id == e.Id) ?? new();

    if (!string.IsNullOrWhiteSpace(GeneralConsultanService.Payment))
    {
        InsurancePolicies = (await Mediator.Send(new GetInsurancePolicyQuery
        {
            Predicate = x => x.UserId == GeneralConsultanService.DiagnosisBPJSIntegrationTempId && x.Insurance != null && x.Insurance.IsBPJS == GeneralConsultanService.Payment.Equals("BPJS") && x.Active == true,
            Select = x => new InsurancePolicy
            {
                Id = x.Id,
                Insurance = new Insurance
                {
                    Name = x.Insurance == null ? "" : x.Insurance.Name,
                },
                NoKartu = x.NoKartu,
                KdProviderPstKdProvider = x.KdProviderPstKdProvider,
                PolicyNumber = x.PolicyNumber,
                PstPrb = x.PstPrb,
                PstProl = x.PstProl
            }
        })).Item1;
    }
}

#endregion ComboboxDiagnosisBPJSIntegrationTemp