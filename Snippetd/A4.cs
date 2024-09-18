var resultOccupationals = await Mediator.Send(new GetOccupationalQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: searchTerm));   
Occupationals = resultOccupationals.Item1;
totalCount = resultOccupationals.pageCount;


var resultOccupationals = await Mediator.Send(new GetOccupationalQuery());
Occupationals = resultOccupationals.Item1;