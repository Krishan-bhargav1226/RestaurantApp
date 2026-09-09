using Application.Dtos.Branches;
using Application.DTOs.Branches;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Branches;

namespace Application.Applications.Branches
{
    public class BranchApplication : IBranchApplication
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public BranchApplication(
            IBranchRepository branchRepository,
            IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }
        public async Task<BranchResponseDto> CreateAsync(CreateUpdateBranchDto input)
        {
            var branch = _mapper.Map<Branch>(input);

            var createdBranch = await _branchRepository.CreateAsync(branch);

            return _mapper.Map<BranchResponseDto>(createdBranch);
        }


        public async Task<List<BranchResponseDto>> GetAllAsync()
        {
            var branches = await _branchRepository.GetAllAsync();

            return _mapper.Map<List<BranchResponseDto>>(branches);
        }



        public async Task<BranchResponseDto> GetByIdAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);

            if (branch == null)
            {
                throw new KeyNotFoundException("Branch not found.");
            }

            return _mapper.Map<BranchResponseDto>(branch);
        }

        public async Task<BranchResponseDto> UpdateAsync(
            int id,
            CreateUpdateBranchDto input)
        {
            var branch = await _branchRepository.GetByIdAsync(id);

            if (branch == null)
            {
                throw new KeyNotFoundException("Branch not found.");
            }

            _mapper.Map(input, branch);

            var updatedBranch = await _branchRepository.UpdateAsync(branch);

            return _mapper.Map<BranchResponseDto>(updatedBranch);
        }

        public async Task DeleteAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);

            if (branch == null)
            {
                throw new KeyNotFoundException("Branch not found.");
            }

            await _branchRepository.DeleteAsync(branch);
        }
    }
}