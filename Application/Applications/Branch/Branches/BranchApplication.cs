using Application.Dtos.Branches;
using Application.DTOs.Branches;
using AutoMapper;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories.Branches;
using Microsoft.EntityFrameworkCore;

namespace Application.Applications.Branches
{
    public class BranchApplication : IBranchApplication
    {
        private readonly IBranchRepository _branchRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BranchApplication(IBranchRepository branchRepository, DataContext context, IMapper mapper)
        {
            _branchRepository = branchRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BranchResponseDto> CreateAsync(CreateUpdateBranchDto input)
        {
            NormalizeAndValidate(input);
            await EnsureUniqueCodeAsync(input.Code);

            var branch = _mapper.Map<Branch>(input);
            var createdBranch = await _branchRepository.CreateAsync(branch);
            return _mapper.Map<BranchResponseDto>(createdBranch);
        }

        public async Task<List<BranchResponseDto>> GetAllAsync() =>
            _mapper.Map<List<BranchResponseDto>>(await _branchRepository.GetAllAsync());

        public async Task<BranchResponseDto> GetByIdAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null)
                throw new KeyNotFoundException("Branch not found.");
            return _mapper.Map<BranchResponseDto>(branch);
        }

        public async Task<BranchResponseDto> UpdateAsync(int id, CreateUpdateBranchDto input)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null)
                throw new KeyNotFoundException("Branch not found.");

            NormalizeAndValidate(input);
            await EnsureUniqueCodeAsync(input.Code, id);
            _mapper.Map(input, branch);
            branch.UpdatedDate = DateTime.UtcNow;

            var updatedBranch = await _branchRepository.UpdateAsync(branch);
            return _mapper.Map<BranchResponseDto>(updatedBranch);
        }

        public async Task DeleteAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null)
                throw new KeyNotFoundException("Branch not found.");
            await _branchRepository.DeleteAsync(branch);
        }

        private async Task EnsureUniqueCodeAsync(string code, int? excludeId = null)
        {
            var exists = await _context.Branches.AnyAsync(x =>
                x.Code == code && (!excludeId.HasValue || x.Id != excludeId.Value));

            if (exists)
                throw new InvalidOperationException("A branch with this code already exists.");
        }

        private static void NormalizeAndValidate(CreateUpdateBranchDto input)
        {
            input.Code = input.Code.Trim().ToUpperInvariant();
            input.Name = input.Name.Trim();
            input.Address = input.Address.Trim();
            input.City = input.City.Trim();
            input.State = input.State.Trim();
            input.PinCode = input.PinCode.Trim();
            input.Phone = input.Phone.Trim();
            input.Email = string.IsNullOrWhiteSpace(input.Email) ? null : input.Email.Trim().ToLowerInvariant();

            if (input.DeliveryRadiusKm is < 0 || input.MinDeliveryOrderAmount is < 0 || input.DeliveryCharge is < 0)
                throw new InvalidOperationException("Delivery radius, minimum order amount and delivery charge cannot be negative.");

            if (input.TaxPercentage is < 0 or > 100)
                throw new InvalidOperationException("Tax percentage must be between 0 and 100.");

            if (input.Latitude is < -90 or > 90 || input.Longitude is < -180 or > 180)
                throw new InvalidOperationException("Latitude or longitude is outside the valid range.");

            if (input.AcceptsDelivery && input.DeliveryCharge is null)
                throw new InvalidOperationException("DeliveryCharge is required when delivery is enabled.");
        }
    }
}