public class GroupCommand
 {
     #region GET

    public class GetSingleGroupQuery : IRequest<GroupDto>
    {
        public List<Expression<Func<Group, object>>> Includes { get; set; }
        public Expression<Func<Group, bool>> Predicate { get; set; }
        public Expression<Func<Group, Group>> Select { get; set; }

        public List<(Expression<Func<Group, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetGroupQuery : IRequest<(List<GroupDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<Group, object>>> Includes { get; set; }
        public Expression<Func<Group, bool>> Predicate { get; set; }
        public Expression<Func<Group, Group>> Select { get; set; }

        public List<(Expression<Func<Group, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateGroup(Expression<Func<Group, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<Group, bool>> Predicate { get; } = predicate!;
     }

     public class BulkValidateGroup(List<GroupDto> GroupsToValidate) : IRequest<List<GroupDto>>
     {
         public List<GroupDto> GroupsToValidate { get; } = GroupsToValidate;
     }

     #endregion GET

     #region CREATE

     public class CreateGroupRequest(GroupDto GroupDto) : IRequest<GroupDto>
     {
         public GroupDto GroupDto { get; set; } = GroupDto;
     }

     public class CreateListGroupRequest(List<GroupDto> GroupDtos) : IRequest<List<GroupDto>>
     {
         public List<GroupDto> GroupDtos { get; set; } = GroupDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateGroupRequest(GroupDto GroupDto) : IRequest<GroupDto>
     {
         public GroupDto GroupDto { get; set; } = GroupDto;
     }

     public class UpdateListGroupRequest(List<GroupDto> GroupDtos) : IRequest<List<GroupDto>>
     {
         public List<GroupDto> GroupDtos { get; set; } = GroupDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteGroupRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateGroupQuery, List<GroupDto>>,
  
IRequestHandler<GetGroupQuery, (List<GroupDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleGroupQuery, GroupDto>,
public class GroupHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetGroupQuery, (List<GroupDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleGroupQuery, GroupDto>, 
     IRequestHandler<ValidateGroup, bool>,
     IRequestHandler<CreateGroupRequest, GroupDto>,
     IRequestHandler<BulkValidateGroup, List<GroupDto>>,
     IRequestHandler<CreateListGroupRequest, List<GroupDto>>,
     IRequestHandler<UpdateGroupRequest, GroupDto>,
     IRequestHandler<UpdateListGroupRequest, List<GroupDto>>,
     IRequestHandler<DeleteGroupRequest, bool>
{
    #region GET
    public async Task<List<GroupDto>> Handle(BulkValidateGroup request, CancellationToken cancellationToken)
    {
        var CountryDtos = request.GroupsToValidate;

        // Ekstrak semua kombinasi yang akan dicari di database
        //var CountryNames = CountryDtos.Select(x => x.Name).Distinct().ToList();
        //var Codes = CountryDtos.Select(x => x.Code).Distinct().ToList();

        //var existingCountrys = await _unitOfWork.Repository<Country>()
        //    .Entities
        //    .AsNoTracking()
        //    .Where(v => CountryNames.Contains(v.Name) && Codes.Contains(v.Code))
        //    .ToListAsync(cancellationToken);

        //return existingCountrys.Adapt<List<CountryDto>>();

        return [];
    }
    public async Task<bool> Handle(ValidateGroup request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<Group>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<GroupDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGroupQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Group>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<Group>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Group>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Group.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Group
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

                return (pagedItems.Adapt<List<GroupDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<GroupDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<GroupDto> Handle(GetSingleGroupQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Group>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<Group>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Group>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.Group.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Group
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GroupDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<GroupDto> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Group>().AddAsync(request.GroupDto.Adapt<CreateUpdateGroupDto>().Adapt<Group>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<GroupDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<GroupDto>> Handle(CreateListGroupRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Group>().AddAsync(request.GroupDtos.Adapt<List<Group>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<GroupDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<GroupDto> Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Group>().UpdateAsync(request.GroupDto.Adapt<GroupDto>().Adapt<Group>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<GroupDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<GroupDto>> Handle(UpdateListGroupRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Group>().UpdateAsync(request.GroupDtos.Adapt<List<Group>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<GroupDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteGroupRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<Group>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<Group>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateGroupQuery(List<GroupDto> GroupsToValidate) : IRequest<List<GroupDto>>
{
    public List<GroupDto> GroupsToValidate { get; } = GroupsToValidate;
}a


IRequestHandler<BulkValidateGroupQuery, List<GroupDto>>,
  
IRequestHandler<GetGroupQuery, (List<GroupDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleGroupQuery, GroupDto>,



 var a = await Mediator.Send(new GetGroupsQuery
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

var patienss = (await Mediator.Send(new GetSingleGroupQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new Group
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
    var result = await Mediator.Send(new GetGroupQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    Groups = result.Item1;
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

 var result = await Mediator.Send(new GetGroupQuery
 {
     Predicate = x => x.CityId == cityId,
     SearchTerm = refGroupComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 Groups = result.Item1;
 totalCountGroup = result.PageCount;

 Groups = (await Mediator.Send(new GetGroupQuery
 {
     Predicate = x => x.Id == GroupForm.IdCardGroupId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleGroupsQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new Group
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new Group
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new Group
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


#region ComboboxGroup

 private DxComboBox<GroupDto, long?> refGroupComboBox { get; set; }
 private int GroupComboBoxIndex { get; set; } = 0;
 private int totalCountGroup = 0;

 private async Task OnSearchGroup()
 {
     await LoadDataGroup();
 }

 private async Task OnSearchGroupIndexIncrement()
 {
     if (GroupComboBoxIndex < (totalCountGroup - 1))
     {
         GroupComboBoxIndex++;
         await LoadDataGroup(GroupComboBoxIndex, 10);
     }
 }

 private async Task OnSearchGroupIndexDecrement()
 {
     if (GroupComboBoxIndex > 0)
     {
         GroupComboBoxIndex--;
         await LoadDataGroup(GroupComboBoxIndex, 10);
     }
 }

 private async Task OnInputGroupChanged(string e)
 {
     GroupComboBoxIndex = 0;
     await LoadDataGroup();
 }

 
  private async Task LoadDataGroup(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetGroupQuery
          {
              SearchTerm = refGroupComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Groups = result.Item1;
          totalCountGroup = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxGroup

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Group" ColSpanMd="12">
    <MyDxComboBox Data="@Groups"
                  NullText="Select Group"
                  @ref="refGroupComboBox"
                  @bind-Value="@a.GroupId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputGroupChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchGroupIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchGroup"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchGroupIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(GroupDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Group.Name" Caption="Group" />
            <DxListEditorColumn FieldName="@nameof(GroupDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.GroupId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<Group>().AddAsync(request.GroupDto.Adapt<CreateUpdateGroupDto>().Adapt<Group>());
var result = await _unitOfWork.Repository<Group>().AddAsync(request.GroupDtos.Adapt<List<CreateUpdateGroupDto>>().Adapt<List<Group>>()); 

var result = await _unitOfWork.Repository<Group>().UpdateAsync(request.GroupDto.Adapt<CreateUpdateGroupDto>().Adapt<Group>());  
var result = await _unitOfWork.Repository<Group>().UpdateAsync(request.GroupDtos.Adapt<List<CreateUpdateGroupDto>>().Adapt<List<Group>>());

list3 = (await Mediator.Send(new GetGroupQuery
{
    Predicate = x => GroupNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new Group
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeGroupAttendance { get; set; } = 10;
    private int totalCountGroupAttendance = 0;
    private int activePageIndexGroupAttendance { get; set; } = 0;
    private string searchTermGroupAttendance { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedGroupAttendance(string searchText)
    {
        searchTermGroupAttendance = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeGroupAttendance);
    }

    private async Task OnpageSizeGroupAttendanceIndexChanged(int newpageSizeGroupAttendance)
    {
        pageSizeGroupAttendance = newpageSizeGroupAttendance;
        await LoadDataOnSearchBoxChanged(0, newpageSizeGroupAttendance);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeGroupAttendance);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeGroupAttendance = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetGroupAttendanceQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeGroupAttendance,
            SearchTerm = searchTermGroupAttendance,
        });
        GroupAttendances = result.Item1;
        totalCountGroupAttendance = result.PageCount;
        activePageIndexGroupAttendance = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}
    #endregion Searching

        <div class="row">
        <DxFormLayout>
            <div class="col-md-9">
                <DxFormLayoutItem>
                    <DxPager PageCount="totalCount"
                             ActivePageIndexChanged="OnPageIndexChanged"
                             ActivePageIndex="activePageIndex"
                             VisibleNumericButtonCount="10"
                             SizeMode="SizeMode.Medium"
                             NavigationMode="PagerNavigationMode.Auto">
                    </DxPager>
                </DxFormLayoutItem>
            </div>
            <div class="col-md-3 d-flex justify-content-end">
                <DxFormLayoutItem Caption="Page Size:">
                    <MyDxComboBox Data="(new[] { 10, 25, 50, 100 })"
                                  NullText="Select Page Size"
                                  ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Never"
                                  SelectedItemChanged="((int e ) => OnPageSizeIndexChanged(e))"
                                  @bind-Value="pageSize">
                    </MyDxComboBox>
                </DxFormLayoutItem>
            </div>
        </DxFormLayout>
    </div>