using Application.Dtos.Branches;
using Application.DTOs.Branches;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Applications.Branches
{
    public interface IBranchApplication
    {
        Task<BranchResponseDto> CreateAsync(CreateUpdateBranchDto input);
        Task<List<BranchResponseDto>> GetAllAsync();
        Task<BranchResponseDto> GetByIdAsync(int id);

        Task<BranchResponseDto> UpdateAsync(int id, CreateUpdateBranchDto input);

        Task DeleteAsync(int id);


    }
}
