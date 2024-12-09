namespace McDermott.Application.Features.Commands
{
    public class GetDataCommand
    {
        public class GetQueryUserlable : BaseQuery<User>
        { }

        #region Configurations

        public class GetQueryGrouplable : BaseQuery<Group>
        { }

        public class GetQueryGroupMenulable : BaseQuery<GroupMenu>
        { }

        public class GetQueryMenulable : BaseQuery<Menu>
        { }

        public class GetQueryCompanylable : BaseQuery<Company>
        { }

        public class GetQueryCountrylable : BaseQuery<Country>
        { }

        public class GetQueryProvincelable : BaseQuery<Province>
        { }

        public class GetQueryCitylable : BaseQuery<City>
        { }

        public class GetQueryOccupationallable : BaseQuery<Occupational>
        { }

        public class GetQueryDistrict : BaseQuery<District>
        { }

        public class GetQueryVillage : BaseQuery<Village>
        { }

        public class GetQueryUser : BaseQuery<User>
        { }

        #endregion Configurations

        #region Inventory
        public class GetQueryGoodReceipt : BaseQuery<GoodsReceipt> { }
        public class GetQueryMaintenance : BaseQuery<Maintenance> { }
        public class GetQueryMaintenanceProduct : BaseQuery<MaintenanceProduct> { }
        #endregion

        // buat di copy"
        //public class GetQuerylable : BaseQuery<>{ }
    }

    public class BaseQuery<TEntity> : IRequest<IQueryable<TEntity>>
    {
        public List<Expression<Func<TEntity, object>>>? Includes { get; set; }
        public Expression<Func<TEntity, bool>>? Predicate { get; set; }
        public Expression<Func<TEntity, TEntity>>? Select { get; set; }
        public List<(Expression<Func<TEntity, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false;
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string? SearchTerm { get; set; }
    }
}