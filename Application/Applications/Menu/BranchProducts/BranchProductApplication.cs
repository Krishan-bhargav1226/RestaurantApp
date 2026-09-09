using Application.Dtos.BranchProducts;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.BranchProducts;

namespace Application.Applications.BranchProducts
{
    public class BranchProductApplication : IBranchProductApplication
    {
        private readonly IBranchProductRepository _branchProductRepository;
        private readonly IMapper _mapper;

        public BranchProductApplication(
            IBranchProductRepository branchProductRepository,
            IMapper mapper)
        {
            _branchProductRepository = branchProductRepository;
            _mapper = mapper;
        }

        public async Task<BranchProductResponseDto> CreateAsync(CreateUpdateBranchProductDto input)
        {
            var branchProduct = _mapper.Map<BranchProduct>(input);

            var createdBranchProduct = await _branchProductRepository.CreateAsync(branchProduct);

            return _mapper.Map<BranchProductResponseDto>(createdBranchProduct);
        }

        public async Task<List<BranchProductResponseDto>> GetAllAsync()
        {
            var branchProducts = await _branchProductRepository.GetAllAsync();

            return _mapper.Map<List<BranchProductResponseDto>>(branchProducts);
        }

        public async Task<BranchProductResponseDto> GetByIdAsync(int id)
        {
            var branchProduct = await _branchProductRepository.GetByIdAsync(id);

            if (branchProduct == null)
            {
                throw new KeyNotFoundException("BranchProduct not found.");
            }

            return _mapper.Map<BranchProductResponseDto>(branchProduct);
        }

        public async Task<BranchProductResponseDto> UpdateAsync(
            int id,
            CreateUpdateBranchProductDto input)
        {
            var branchProduct = await _branchProductRepository.GetByIdAsync(id);

            if (branchProduct == null)
            {
                throw new KeyNotFoundException("BranchProduct not found.");
            }

            _mapper.Map(input, branchProduct);

            var updatedBranchProduct = await _branchProductRepository.UpdateAsync(branchProduct);

            return _mapper.Map<BranchProductResponseDto>(updatedBranchProduct);
        }

        public async Task DeleteAsync(int id)
        {
            var branchProduct = await _branchProductRepository.GetByIdAsync(id);

            if (branchProduct == null)
            {
                throw new KeyNotFoundException("BranchProduct not found.");
            }

            await _branchProductRepository.DeleteAsync(branchProduct);
        }
    }
}
