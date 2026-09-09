using Application.Dtos.Branches;
using Application.DTOs.Branches;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Common.Mapping
{
    public class BranchProfile: Profile
    {
        public BranchProfile()
        {
            CreateMap<CreateUpdateBranchDto, Branch>();

            CreateMap<Branch, BranchResponseDto>();
        }
    }
}
