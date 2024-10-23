 public class GetSingleProjectQuery : IRequest<ProjectDto>
 {
     public List<Expression<Func<Project, object>>> Includes { get; set; }
     public Expression<Func<Project, bool>> Predicate { get; set; }
     public Expression<Func<Project, Project>> Select { get; set; }

     public List<(Expression<Func<Project, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

     public bool IsDescending { get; set; } = false; // default to ascending
     public int PageIndex { get; set; } = 0;
     public int PageSize { get; set; } = 10;
     public bool IsGetAll { get; set; } = false;
     public string SearchTerm { get; set; }
 }

public class GetProjectQuery : IRequest<(List<ProjectDto>, int PageIndex, int PageSize, int PageCount)>
{
    public List<Expression<Func<Project, object>>> Includes { get; set; }
    public Expression<Func<Project, bool>> Predicate { get; set; }
    public Expression<Func<Project, Project>> Select { get; set; }

    public List<(Expression<Func<Project, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

    public bool IsDescending { get; set; } = false; // default to ascending
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public bool IsGetAll { get; set; } = false;
    public string SearchTerm { get; set; }
}
  
IRequestHandler<GetProjectQuery, (List<ProjectDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleProjectQuery, ProjectDto>,

public async Task<(List<ProjectDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetProjectQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<Project>().Entities.AsNoTracking(); 

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
                    ? ((IOrderedQueryable<Project>)query).ThenByDescending(additionalOrderBy.OrderBy)
                    : ((IOrderedQueryable<Project>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.Project.Name, $"%{request.SearchTerm}%")
                    );
        }

        // Apply dynamic select if provided
        if (request.Select is not null)
            query = query.Select(request.Select);
        else
            query = query.Select(x => new Project
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

            return (pagedItems.Adapt<List<ProjectDto>>(), request.PageIndex, request.PageSize, totalPages);
        }
        else
        {
            return ((await query.ToListAsync(cancellationToken)).Adapt<List<ProjectDto>>(), 0, 1, 1);
        }
    }
    catch (Exception ex)
    {
        // Consider logging the exception
        throw;
    }
}


  public async Task<ProjectDto> Handle(GetSingleProjectQuery request, CancellationToken cancellationToken)
 {
     try
     {
         var query = _unitOfWork.Repository<Project>().Entities.AsNoTracking();

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
                     ? ((IOrderedQueryable<Project>)query).ThenByDescending(additionalOrderBy.OrderBy)
                     : ((IOrderedQueryable<Project>)query).ThenBy(additionalOrderBy.OrderBy);
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
                EF.Functions.Like(v.Project.Name, $"%{request.SearchTerm}%")
                );
         }

         // Apply dynamic select if provided
         if (request.Select is not null)
             query = query.Select(request.Select);
         else
             query = query.Select(x => new Project
             {
                 Id = x.Id, 
             });

         return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<ProjectDto>();
     }
     catch (Exception ex)
     {
         // Consider logging the exception
         throw;
     }
 }




 var a = await Mediator.Send(new GetProjectsQuery
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

var patienss = (await Mediator.Send(new GetSingleProjectQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new Project
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
    var result = await Mediator.Send(new GetProjectQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    Projects = result.Item1;
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






var data = (await Mediator.Send(new GetSingleProjectsQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new Project
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new Project
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new Project
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


#region ComboboxProject

 private DxComboBox<ProjectDto, long?> refProjectComboBox { get; set; }
 private int ProjectComboBoxIndex { get; set; } = 0;
 private int totalCountProject = 0;

 private async Task OnSearchProject()
 {
     await LoadDataProject();
 }

 private async Task OnSearchProjectIndexIncrement()
 {
     if (ProjectComboBoxIndex < (totalCountProject - 1))
     {
         ProjectComboBoxIndex++;
         await LoadDataProject(ProjectComboBoxIndex, 10);
     }
 }

 private async Task OnSearchProjectIndexDecrement()
 {
     if (ProjectComboBoxIndex > 0)
     {
         ProjectComboBoxIndex--;
         await LoadDataProject(ProjectComboBoxIndex, 10);
     }
 }

 private async Task OnInputProjectChanged(string e)
 {
     ProjectComboBoxIndex = 0;
     await LoadDataProject();
 }



  private async Task LoadDataProject(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetProjectQuery
          {
              SearchTerm = refProjectComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Projects = result.Item1;
          totalCountProject = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxProject

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Project" ColSpanMd="12">
    <MyDxComboBox Data="@Projects"
                  NullText="Select Project"
                  @ref="refProjectComboBox"
                  @bind-Value="@a.ProjectId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputProjectChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchProjectIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchProject"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchProjectIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(ProjectDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Project.Name" Caption="Project" />
            <DxListEditorColumn FieldName="@nameof(ProjectDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.ProjectId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<Accident>().AddAsync(request.AccidentDto.Adapt<CreateUpdateAccidentDto>().Adapt<Accident>());
var result = await _unitOfWork.Repository<Accident>().AddAsync(request.AccidentDtos.Adapt<List<CreateUpdateAccidentDto>>().Adapt<List<Accident>>()); 

var result = await _unitOfWork.Repository<Accident>().UpdateAsync(request.AccidentDto.Adapt<CreateUpdateAccidentDto>().Adapt<Accident>());  
var result = await _unitOfWork.Repository<Accident>().UpdateAsync(request.AccidentDtos.Adapt<List<CreateUpdateAccidentDto>>().Adapt<List<Accident>>());

