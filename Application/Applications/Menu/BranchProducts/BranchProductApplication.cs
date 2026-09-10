using Application.Dtos.BranchProducts;
using AutoMapper;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories.BranchProducts;
using Microsoft.EntityFrameworkCore;

namespace Application.Applications.BranchProducts
{
    public class BranchProductApplication : IBranchProductApplication
    {
        private readonly IBranchProductRepository _branchProductRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BranchProductApplication(
            IBranchProductRepository branchProductRepository,
            DataContext context,
            IMapper mapper)
        {
            _branchProductRepository = branchProductRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BranchProductResponseDto> CreateAsync(CreateUpdateBranchProductDto input)
        {
            await EnsureReferencesAsync(input.BranchId, input.ProductId);

            var duplicate = await _context.BranchProducts.AnyAsync(x =>
                x.BranchId == input.BranchId &&
                x.ProductId == input.ProductId);

            if (duplicate)
                throw new InvalidOperationException("This product is already configured for the branch.");

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
                throw new KeyNotFoundException("BranchProduct not found.");

            return _mapper.Map<BranchProductResponseDto>(branchProduct);
        }

        public async Task<BranchProductResponseDto> UpdateAsync(int id, CreateUpdateBranchProductDto input)
        {
            var branchProduct = await _branchProductRepository.GetByIdAsync(id);

            if (branchProduct == null)
                throw new KeyNotFoundException("BranchProduct not found.");

            await EnsureReferencesAsync(input.BranchId, input.ProductId);

            var duplicate = await _context.BranchProducts.AnyAsync(x =>
                x.Id != id &&
                x.BranchId == input.BranchId &&
                x.ProductId == input.ProductId);

            if (duplicate)
                throw new InvalidOperationException("This product is already configured for the branch.");

            _mapper.Map(input, branchProduct);
            var updatedBranchProduct = await _branchProductRepository.UpdateAsync(branchProduct);
            return _mapper.Map<BranchProductResponseDto>(updatedBranchProduct);
        }

        public async Task DeleteAsync(int id)
        {
            var branchProduct = await _branchProductRepository.GetByIdAsync(id);

            if (branchProduct == null)
                throw new KeyNotFoundException("BranchProduct not found.");

            await _branchProductRepository.DeleteAsync(branchProduct);
        }

        private async Task EnsureReferencesAsync(int branchId, int productId)
        {
            var branchExists = await _context.Branches.AnyAsync(x => x.Id == branchId);
            if (!branchExists)
                throw new KeyNotFoundException("Branch not found.");

            var productExists = await _context.Products.AnyAsync(x => x.Id == productId);
            if (!productExists)
                throw new KeyNotFoundException("Product not found.");
        }
    }
}
