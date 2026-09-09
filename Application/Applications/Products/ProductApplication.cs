using Application.Dtos.Products;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Products;

namespace Application.Applications.Products
{
    public class ProductApplication : IProductApplication
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductApplication(
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductResponseDto> CreateAsync(CreateUpdateProductDto input)
        {
            var product = _mapper.Map<Product>(input);

            var createdProduct = await _productRepository.CreateAsync(product);

            return _mapper.Map<ProductResponseDto>(createdProduct);
        }

        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return _mapper.Map<List<ProductResponseDto>>(products);
        }

        public async Task<ProductResponseDto> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<ProductResponseDto> UpdateAsync(
            int id,
            CreateUpdateProductDto input)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            _mapper.Map(input, product);

            var updatedProduct = await _productRepository.UpdateAsync(product);

            return _mapper.Map<ProductResponseDto>(updatedProduct);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            await _productRepository.DeleteAsync(product);
        }
    }
}
