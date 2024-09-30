private async Task LoadDataProvince(int pageIndex = 0, int pageSize = 10)
 {
     PanelVisible = true;
     var result = await Mediator.Send(new GetProvinceQuery(
         pageIndex: pageIndex,
         pageSize: pageSize,
         searchTerm: refProvinceComboBox?.Text ?? "",
         includes:
         [
             x => x.Department
         ],
         select: x => new Province
         {
             Id = x.Id,
             Name = x.Name,
             Department = new Domain.Entities.Department
             {
                 Name = x.Name
             },
         }

     ));
     Provinces = result.Item1;
     totalCountProvince = result.pageCount;
     PanelVisible = false;
 }

var result = await Mediator.Send(new GetProvinceQuery(
    pageIndex: pageIndex,
    pageSize: pageSize,
    searchTerm: refProvinceComboBox?.Text ?? "",
    includes:
    [
        x => x.Department
    ],
    select: x => new Province
    {
        Id = x.Id,
        Name = x.Name,
        Department = new Domain.Entities.Department
        {
            Name = x.Name
        },
    }

));

Province