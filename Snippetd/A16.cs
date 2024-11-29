ProductCategory

public class ProductCategoryCommand
 {
     #region GET

    public class GetSingleProductCategoryQuery : IRequest<ProductCategoryDto>
    {
        public List<Expression<Func<ProductCategory, object>>> Includes { get; set; }
        public Expression<Func<ProductCategory, bool>> Predicate { get; set; }
        public Expression<Func<ProductCategory, ProductCategory>> Select { get; set; }

        public List<(Expression<Func<ProductCategory, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetProductCategoryQuery : IRequest<(List<ProductCategoryDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<ProductCategory, object>>> Includes { get; set; }
        public Expression<Func<ProductCategory, bool>> Predicate { get; set; }
        public Expression<Func<ProductCategory, ProductCategory>> Select { get; set; }

        public List<(Expression<Func<ProductCategory, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateProductCategory(Expression<Func<ProductCategory, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<ProductCategory, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateProductCategoryRequest(ProductCategoryDto ProductCategoryDto) : IRequest<ProductCategoryDto>
     {
         public ProductCategoryDto ProductCategoryDto { get; set; } = ProductCategoryDto;
     }

     public class BulkValidateProductCategory(List<ProductCategoryDto> ProductCategorysToValidate) : IRequest<List<ProductCategoryDto>>
     {
         public List<ProductCategoryDto> ProductCategorysToValidate { get; } = ProductCategorysToValidate;
     }

     public class CreateListProductCategoryRequest(List<ProductCategoryDto> ProductCategoryDtos) : IRequest<List<ProductCategoryDto>>
     {
         public List<ProductCategoryDto> ProductCategoryDtos { get; set; } = ProductCategoryDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateProductCategoryRequest(ProductCategoryDto ProductCategoryDto) : IRequest<ProductCategoryDto>
     {
         public ProductCategoryDto ProductCategoryDto { get; set; } = ProductCategoryDto;
     }

     public class UpdateListProductCategoryRequest(List<ProductCategoryDto> ProductCategoryDtos) : IRequest<List<ProductCategoryDto>>
     {
         public List<ProductCategoryDto> ProductCategoryDtos { get; set; } = ProductCategoryDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteProductCategoryRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateProductCategoryQuery, List<ProductCategoryDto>>,
  
IRequestHandler<GetProductCategoryQuery, (List<ProductCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleProductCategoryQuery, ProductCategoryDto>,
public class ProductCategoryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetProductCategoryQuery, (List<ProductCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleProductCategoryQuery, ProductCategoryDto>, 
     IRequestHandler<ValidateProductCategory, bool>,
     IRequestHandler<CreateProductCategoryRequest, ProductCategoryDto>,
     IRequestHandler<BulkValidateProductCategory, List<ProductCategoryDto>>,
     IRequestHandler<CreateListProductCategoryRequest, List<ProductCategoryDto>>,
     IRequestHandler<UpdateProductCategoryRequest, ProductCategoryDto>,
     IRequestHandler<UpdateListProductCategoryRequest, List<ProductCategoryDto>>,
     IRequestHandler<DeleteProductCategoryRequest, bool>
{
    #region GET
    public async Task<List<ProductCategoryDto>> Handle(BulkValidateProductCategory request, CancellationToken cancellationToken)
    {
        var ProductCategoryDtos = request.ProductCategorysToValidate;

        // Ekstrak semua kombinasi yang akan dicari di database
        //var ProductCategoryNames = ProductCategoryDtos.Select(x => x.Name).Distinct().ToList();
        //var Codes = ProductCategoryDtos.Select(x => x.Code).Distinct().ToList();

        //var existingProductCategorys = await _unitOfWork.Repository<ProductCategory>()
        //    .Entities
        //    .AsNoTracking()
        //    .Where(v => ProductCategoryNames.Contains(v.Name) && Codes.Contains(v.Code))
        //    .ToListAsync(cancellationToken);

        //return existingProductCategorys.Adapt<List<ProductCategoryDto>>();

        return [];
    }
    public async Task<bool> Handle(ValidateProductCategory request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<ProductCategory>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<ProductCategoryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetProductCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<ProductCategory>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<ProductCategory>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<ProductCategory>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.ProductCategory.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new ProductCategory
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

                return (pagedItems.Adapt<List<ProductCategoryDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<ProductCategoryDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<ProductCategoryDto> Handle(GetSingleProductCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<ProductCategory>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<ProductCategory>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<ProductCategory>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.ProductCategory.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new ProductCategory
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<ProductCategoryDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<ProductCategoryDto> Handle(CreateProductCategoryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<ProductCategory>().AddAsync(request.ProductCategoryDto.Adapt<CreateUpdateProductCategoryDto>().Adapt<ProductCategory>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetProductCategoryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<ProductCategoryDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<ProductCategoryDto>> Handle(CreateListProductCategoryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<ProductCategory>().AddAsync(request.ProductCategoryDtos.Adapt<List<ProductCategory>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetProductCategoryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<ProductCategoryDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<ProductCategoryDto> Handle(UpdateProductCategoryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<ProductCategory>().UpdateAsync(request.ProductCategoryDto.Adapt<ProductCategoryDto>().Adapt<ProductCategory>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetProductCategoryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<ProductCategoryDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<ProductCategoryDto>> Handle(UpdateListProductCategoryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<ProductCategory>().UpdateAsync(request.ProductCategoryDtos.Adapt<List<ProductCategory>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetProductCategoryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<ProductCategoryDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteProductCategoryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<ProductCategory>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<ProductCategory>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetProductCategoryQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateProductCategoryQuery(List<ProductCategoryDto> ProductCategorysToValidate) : IRequest<List<ProductCategoryDto>>
{
    public List<ProductCategoryDto> ProductCategorysToValidate { get; } = ProductCategorysToValidate;
}a


IRequestHandler<BulkValidateProductCategoryQuery, List<ProductCategoryDto>>,
  
IRequestHandler<GetProductCategoryQuery, (List<ProductCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleProductCategoryQuery, ProductCategoryDto>,



 var a = await Mediator.Send(new GetProductCategorysQuery
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

var patienss = (await Mediator.Send(new GetSingleProductCategoryQuery
{
    Predicate = x => x.Id == data.DiagnosisBPJSIntegrationTempId,
    Select = x => new ProductCategory
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
    var result = await Mediator.Send(new GetProductCategoryQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    ProductCategorys = result.Item1;
    totalCount = result.PageCount;
    activePageIndex = pageIndex;
}
catch (Exception ex)
{
    ex.HandleException(ToastProductCategory);
}
finally
{ 
    PanelVisible = false;
}

 var result = await Mediator.Send(new GetProductCategoryQuery
 {
     Predicate = x => x.ProductCategoryId == ProductCategoryId,
     SearchTerm = refProductCategoryComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 ProductCategorys = result.Item1;
 totalCountProductCategory = result.PageCount;

 ProductCategorys = (await Mediator.Send(new GetProductCategoryQuery
 {
     Predicate = x => x.Id == ProductCategoryForm.IdCardProductCategoryId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleProductCategorysQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.DiagnosisBPJSIntegrationTemp
    ],
    Select = x => new ProductCategory
    {
        Id = x.Id,
        DiagnosisBPJSIntegrationTempId = x.DiagnosisBPJSIntegrationTempId,
        DiagnosisBPJSIntegrationTemp = new ProductCategory
        {
            DateOfBirth = x.DiagnosisBPJSIntegrationTemp.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new ProductCategory
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


#region ComboboxProductCategory

 private DxComboBox<ProductCategoryDto, long?> refProductCategoryComboBox { get; set; }
 private int ProductCategoryComboBoxIndex { get; set; } = 0;
 private int totalCountProductCategory = 0;

 private async Task OnSearchProductCategory()
 {
     await LoadDataProductCategory();
 }

 private async Task OnSearchProductCategoryIndexIncrement()
 {
     if (ProductCategoryComboBoxIndex < (totalCountProductCategory - 1))
     {
         ProductCategoryComboBoxIndex++;
         await LoadDataProductCategory(ProductCategoryComboBoxIndex, 10);
     }
 }

 private async Task OnSearchProductCategoryIndexDecrement()
 {
     if (ProductCategoryComboBoxIndex > 0)
     {
         ProductCategoryComboBoxIndex--;
         await LoadDataProductCategory(ProductCategoryComboBoxIndex, 10);
     }
 }

 private async Task OnInputProductCategoryChanged(string e)
 {
     ProductCategoryComboBoxIndex = 0;
     await LoadDataProductCategory();
 }

 
  private async Task LoadDataProductCategory(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetProductCategoryQuery
          {
              SearchTerm = refProductCategoryComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          ProductCategorys = result.Item1;
          totalCountProductCategory = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastProductCategory);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxProductCategory

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="ProductCategory" ColSpanMd="12">
    <MyDxComboBox Data="@ProductCategorys"
                  NullText="Select ProductCategory"
                  @ref="refProductCategoryComboBox"
                  @bind-Value="@a.ProductCategoryId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputProductCategoryChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchProductCategoryIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchProductCategory"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchProductCategoryIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(ProductCategoryDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="ProductCategory.Name" Caption="ProductCategory" />
            <DxListEditorColumn FieldName="@nameof(ProductCategoryDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.ProductCategoryId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<ProductCategory>().AddAsync(request.ProductCategoryDto.Adapt<CreateUpdateProductCategoryDto>().Adapt<ProductCategory>());
var result = await _unitOfWork.Repository<ProductCategory>().AddAsync(request.ProductCategoryDtos.Adapt<List<CreateUpdateProductCategoryDto>>().Adapt<List<ProductCategory>>()); 

var result = await _unitOfWork.Repository<ProductCategory>().UpdateAsync(request.ProductCategoryDto.Adapt<CreateUpdateProductCategoryDto>().Adapt<ProductCategory>());  
var result = await _unitOfWork.Repository<ProductCategory>().UpdateAsync(request.ProductCategoryDtos.Adapt<List<CreateUpdateProductCategoryDto>>().Adapt<List<ProductCategory>>());

list3 = (await Mediator.Send(new GetProductCategoryQuery
{
    Predicate = x => ProductCategoryNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new ProductCategory
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeProductCategory { get; set; } = 10;
    private int totalCountProductCategory = 0;
    private int activePageIndexProductCategory { get; set; } = 0;
    private string searchTermProductCategory { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedProductCategory(string searchText)
    {
        searchTermProductCategory = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeProductCategory);
    }

    private async Task OnpageSizeProductCategoryIndexChanged(int newpageSizeProductCategory)
    {
        pageSizeProductCategory = newpageSizeProductCategory;
        await LoadDataOnSearchBoxChanged(0, newpageSizeProductCategory);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeProductCategory);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeProductCategory = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetProductCategoryQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeProductCategory,
            SearchTerm = searchTermProductCategory,
        });
        ProductCategorys = result.Item1;
        totalCountProductCategory = result.PageCount;
        activePageIndexProductCategory = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastProductCategory);
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
          var a = await Mediator.Send(new GetGeneralConsultanProductCategorysQuery
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

          GeneralConsultanProductCategorys = a.Item1;
          totalCount = a.PageCount;
          activePageIndex = pageIndex;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastProductCategory);
      }
      finally { PanelVisible = false; }
  }

  #endregion Searching


   <MyGridPaginate @ref="GridDetail"
                 Data="ProductCategorys"
                 @bind-SelectedDataItems="@SelectedDetailDataItems"
                 EditModelSaving="OnSaveInventoryAdjumentDetail"
                 DataItemDeleting="OnDeleteInventoryAdjumentDetail"
                 EditFormButtonsVisible="false"
                 FocusedRowChanged="GridDetail_FocusedRowChanged"
                 SearchTextChanged="OnSearchBoxChanged"
                 KeyFieldName="Id">


     <ToolbarTemplate>
         <MyDxToolbarBase TItem="ProductCategoryDto"
                          Items="@ProductCategorys"
                          Grid="GridDetail"
                          SelectedDataItems="@SelectedDetailDataItems"
                          NewItem_Click="@NewItem_Click"
                          EditItem_Click="@EditItem_Click"
                          DeleteItem_Click="@DeleteItem_Click"
                          Refresh_Click="@(async () => await LoadData())"
                          IsImport="ProductCategoryAccessCRUID.IsImport"
                          VisibleNew="ProductCategoryAccessCRUID.IsCreate"
                          VisibleEdit="ProductCategoryAccessCRUID.IsUpdate"
                          VisibleDelete="ProductCategoryAccessCRUID.IsDelete" />
     </ToolbarTemplate>


     <Columns>
         <DxGridSelectionColumn Width="15px" />
         <DxGridDataColumn FieldName="ProductCategory.Name" Caption="ProductCategory"></DxGridDataColumn>
         <DxGridDataColumn FieldName="TeoriticalQty" Caption="Teoritical Qty" />
         <DxGridDataColumn FieldName="RealQty" Caption="Real Qty" />
         <DxGridDataColumn FieldName="Difference" Caption="Difference" />
         <DxGridDataColumn FieldName="Batch" Caption="Lot Serial Number" />
         <DxGridDataColumn FieldName="ExpiredDate" Caption="Expired Date" SortIndex="0" DisplayFormat="@Helper.DefaultFormatDate" />
         <DxGridDataColumn FieldName="ProductCategory.ProductCategory.Name" Caption="ProductCategory" />
     </Columns>
     <EditFormTemplate Context="EditFormContext">
         @{
             if (EditFormContext.DataItem is null)
             {
                 FormProductCategory = (ProductCategoryDto)EditFormContext.EditModel;
             }
             var IsBatch = ProductCategorys.FirstOrDefault(x => x.Id == FormProductCategory.ProductCategoryId)?.TraceAbility ?? false;

             ActiveButton = FormProductCategory.ProductCategoryId is null ||
             string.IsNullOrWhiteSpace(FormProductCategory.Batch) && IsBatch ||
             FormProductCategory.ExpiredDate is null ||
             FormProductCategory.ProductCategoryId is null;
         }
         <div class="row w-100">
             <DxFormLayout CssClass="w-100">
                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="ProductCategory" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox Data="@ProductCategorys"
                                     @bind-Value="@FormProductCategory.ProductCategoryId"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     NullText="Select ProductCategory..."
                                     TextFieldName="Name"
                                     ReadOnly="@(FormProductCategory.Id != 0)"
                                     ValueFieldName="Id"
                                     SelectedItemChanged="@(async (ProductCategoryDto freq) => await OnSelectProductCategory(freq))"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     ShowValidationIcon="true" />
                         <ValidationMessage For="@(()=> FormProductCategory.ProductCategoryId)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem Caption="Batch" Enabled="FormProductCategory.Id == 0" Visible="IsBatch" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <MyDxComboBox Data="@Batch"
                                       ReadOnly="@(FormProductCategory.Id != 0)"
                                       NullText="Select Batch..."
                                       AllowProductCategoryInput="true"
                                       @bind-Value="@FormProductCategory.Batch"
                                       @bind-Text="@FormProductCategory.Batch"
                                       SelectedItemChanged="@((string a)=> SelectedBatch(a))" />
                         <ValidationMessage For="@(() => FormProductCategory.Batch)" />

                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Teoritical Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     ReadOnly
                                     MinValue="0"
                                     @bind-Value="@FormProductCategory.TeoriticalQty"
                                     NullText="Teoritical Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormProductCategory.TeoriticalQty)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Real Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     MinValue="0"
                                     @bind-Value="@FormProductCategory.RealQty"
                                     NullText="Real Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormProductCategory.RealQty)"   />
                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="Expired Date" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxDateEdit ShowValidationIcon="true"
                                     ReadOnly="@(FormProductCategory.Id != 0)"
                                     DisplayFormat="@Helper.DefaultFormatDate"
                                     @bind-Date="@FormProductCategory.ExpiredDate"
                                     NullText="Expired Date">
                         </DxDateEdit>
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption required-caption" Caption="ProductCategory" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox ShowValidationIcon="true" Data="@ProductCategorys"
                                     NullText="ProductCategory"
                                     ReadOnly="@(FormProductCategory.Id != 0)"
                                     TextFieldName="Name"
                                     ValueFieldName="Id"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     @bind-Value="FormProductCategory.ProductCategoryId">
                         </DxComboBox>
                         <ValidationMessage For="@(() => FormProductCategory.ProductCategoryId)" />
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

ProductCategory

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="ProductCategory" ColSpanMd="12">
    <DxComboBox Data="ProductCategorys"
                AllowProductCategoryInput="true"
                NullText="Select ProductCategory"
                ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputProductCategory"
                @bind-Value="a.ProductCategoryId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(ProductCategory.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(ProductCategory.Code)" Caption="Code" />
        </Columns>
    </DxComboBox>
    <ValidationMessage For="@(()=>a.ProductCategoryId)" />
</DxFormLayoutItem>

#region ComboBox ProductCategory
 
private CancellationTokenSource? _ctsProductCategory;
private async Task OnInputProductCategory(ChangeEventArgs e)
{
    try
    {
        PanelVisible = true;
            
        _ctsProductCategory?.Cancel();
        _ctsProductCategory?.Dispose();
        _ctsProductCategory = new CancellationTokenSource();
            
        await Task.Delay(700, _ctsProductCategory.Token);
            
        await LoadProductCategory(e.Value?.ToString() ?? "");
    } 
    finally
    {
        PanelVisible = false;

        // Untuk menghindari kebocoran memori (memory leaks).
        _ctsProductCategory?.Dispose();
        _ctsProductCategory = null;
    } 
}

 private async Task LoadProductCategory(string? e = "", Expression<Func<ProductCategory, bool>>? predicate = null)
 {
     try
     {
         PanelVisible = true;
         ProductCategorys = await Mediator.QueryGetComboBox<ProductCategory, ProductCategoryDto>(e, predicate);
         PanelVisible = false;
     }
     catch (Exception ex)
     {
         ex.HandleException(ToastProductCategory);
     }
     finally { PanelVisible = false; }
 }

#endregion


// Ini buat di EditItemClick
await LoadProductCategory(id:  a.ProductCategoryId);

ProductCategory



VIRAL 2025

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="ProductCategory" ColSpanMd="12">
    <MyDxComboBox Data="ProductCategorys"
                NullText="Select ProductCategory"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputProductCategory"
                ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                SelectedItemChanged="((ProductCategoryDto e) => SelectedItemChanged(e))"  
                @bind-Value="a.ProductCategoryId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(ProductCategory.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(ProductCategory.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.ProductCategoryId)" />
</DxFormLayoutItem>


#region ComboBox ProductCategory

    private ProductCategoryDto SelectedProductCategory { get; set; } = new();
    async Task SelectedItemChanged(ProductCategoryDto e)
    {
        if (e is null)
        {
            SelectedProductCategory = new();
            await LoadProductCategory(); 
        }
        else
            SelectedProductCategory = e;
    }

    private CancellationTokenSource? _ctsProductCategory;
    private async Task OnInputProductCategory(ChangeEventArgs e)
    {
        try
        { 
            _ctsProductCategory?.Cancel();
            _ctsProductCategory?.Dispose();
            _ctsProductCategory = new CancellationTokenSource();

            await Task.Delay(Helper.CBX_DELAY, _ctsProductCategory.Token);

            await LoadProductCategory(e.Value?.ToString() ?? "");
        }
        finally
        { 
            _ctsProductCategory?.Dispose();
            _ctsProductCategory = null;
        }
    }

    private async Task LoadProductCategory(string? e = "", Expression<Func<ProductCategory, bool>>? predicate = null)
    {
        try
        { 
            ProductCategorys = await Mediator.QueryGetComboBox<ProductCategory, ProductCategoryDto>(e, predicate); 
        }
        catch (Exception ex)
        {
            ex.HandleException(ToastProductCategory);
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