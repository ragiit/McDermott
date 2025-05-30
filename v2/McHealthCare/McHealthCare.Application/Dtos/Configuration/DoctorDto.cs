﻿namespace McHealthCare.Application.Dtos.Configuration
{
    public class DoctorDto : IMapFrom<Doctor>
    {
        public Guid? SpecialityId { get; set; }
        public string? ApplicationUserId { get; set; }
        public bool DoctorType { get; set; }
        private bool _isPhysicion = false;
        private bool _isNurse = false;

        public bool IsPhysicion
        {
            get => _isPhysicion;
            set
            {
                _isPhysicion = value;
                if (value == true)
                {
                    _isNurse = false;
                }
            }
        }

        public bool IsNurse
        {
            get => _isNurse;
            set
            {
                _isNurse = value;
                if (value == true)
                {
                    _isPhysicion = false;
                }
            }
        }

        public string? PhysicanCode { get; set; }
        public string? SipNo { get; set; }
        public string? SipFile { get; set; }
        public string? SipFileBase64 { get; set; }
        public DateTime? SipExp { get; set; }
        public string? StrNo { get; set; }
        public string? StrFile { get; set; }
        public string? StrFileBase64 { get; set; }
        public DateTime? StrExp { get; set; }
    }

    public class CreateUpdateDoctorDto
    {
        public Guid? SpecialityId { get; set; }
        public string? ApplicationUserId { get; set; } = Guid.NewGuid().ToString();
        public bool DoctorType { get; set; }
        private bool _isPhysicion = false;
        private bool _isNurse = false;

        public bool IsPhysicion
        {
            get => _isPhysicion;
            set
            {
                _isPhysicion = value;
                if (value == true)
                {
                    _isNurse = false;
                }
            }
        }

        public bool IsNurse
        {
            get => _isNurse;
            set
            {
                _isNurse = value;
                if (value == true)
                {
                    _isPhysicion = false;
                }
            }
        }

        public string? PhysicanCode { get; set; }
        public string? SipNo { get; set; }
        public string? SipFile { get; set; }
        public string? SipFileBase64 { get; set; }
        public DateTime? SipExp { get; set; }
        public string? StrNo { get; set; }
        public string? StrFile { get; set; }
        public string? StrFileBase64 { get; set; }
        public DateTime? StrExp { get; set; }
    }
}