private async Task LoadDataCity(int pageIndex = 0, int pageSize = 10)
 {
     PanelVisible = true;
     var result = await Mediator.Send(new GetCityQuery(
         pageIndex: pageIndex,
         pageSize: pageSize,
         searchTerm: refCityComboBox?.Text ?? "",
         includes:
         [
             x => x.Department
         ],
         select: x => new City
         {
             Id = x.Id,
             Name = x.Name,
             Department = new Domain.Entities.Department
             {
                 Name = x.Name
             },
         }

     ));
     Citys = result.Item1;
     totalCountCity = result.pageCount;
     PanelVisible = false;
 }

var result = await Mediator.Send(new GetCityQuery(
    pageIndex: pageIndex,
    pageSize: pageSize,
    searchTerm: refCityComboBox?.Text ?? "",
    includes:
    [
        x => x.Department
    ],
    select: x => new City
    {
        Id = x.Id,
        Name = x.Name,
        Department = new Domain.Entities.Department
        {
            Name = x.Name
        },
    }

));

City