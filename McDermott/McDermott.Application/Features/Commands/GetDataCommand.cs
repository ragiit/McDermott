namespace McDermott.Application.Features.Commands
{
    public class GetDataCommand
    {
        public class GetQueryUser : BaseQuery<User>
        { }

        #region Transactions

        public class GetQueryGeneralConsultanServiceCPPT : BaseQuery<GeneralConsultanCPPT>
        { }

        public class GetQueryGeneralConsultanService : BaseQuery<GeneralConsultanService>
        { }

        #endregion Transactions

        #region Configurations

        public class GetQueryGroup : BaseQuery<Group>
        { }

        public class GetQueryGroupMenu : BaseQuery<GroupMenu>
        { }

        public class GetQueryMenu : BaseQuery<Menu>
        { }

        public class GetQueryCompany : BaseQuery<Company>
        { }

        public class GetQueryCountry : BaseQuery<Country>
        { }

        public class GetQueryProvince : BaseQuery<Province>
        { }

        public class GetQueryCity : BaseQuery<City>
        { }

        public class GetQueryOccupational : BaseQuery<Occupational>
        { }

        public class GetQueryDistrict : BaseQuery<District>
        { }

        public class GetQueryVillage : BaseQuery<Village>
        { }

        #endregion Configurations

        #region Medicals

        public class GetQueryDiagnosis : BaseQuery<Diagnosis>
        { }

        public class GetQuerySpeciality : BaseQuery<Speciality>
        { }

        public class GetQueryService : BaseQuery<Service>
        { }

        #endregion Medicals

        #region Inventory

        public class GetQueryGoodReceipt : BaseQuery<GoodsReceipt>
        { }

        public class GetQueryMaintenance : BaseQuery<Maintenance>
        { }

        public class GetQueryMaintenanceProduct : BaseQuery<MaintenanceProduct>
        { }

        public class GetQueryMaintenanceRecord : BaseQuery<MaintenanceRecord>
        { }

        #endregion Inventory

        #region Patients

        public class GetQueryPatientFamilyRelation : BaseQuery<PatientFamilyRelation>
        { }

        #endregion Patients

        #region Employees

        public class GetQueryJobPosition : BaseQuery<JobPosition>
        { }

        public class GetQueryDepartment : BaseQuery<Department>
        { }

        #endregion Employees

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