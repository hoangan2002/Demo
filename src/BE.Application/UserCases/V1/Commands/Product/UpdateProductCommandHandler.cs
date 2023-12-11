﻿using BE.Contract.Abstractions.Message;
using BE.Contract.Abstractions.Shared;
using BE.Domain.Abstractions.Repositories;
using BE.Domain.Abstractions;
using BE.Domain.Exceptions;
using BE.Persistance;
using BE.Contract.Services.Product;

namespace BE.Application.UserCases.V1.Commands.Product;
public sealed class UpdateProductCommandHandler : ICommandHandler<Command.UpdateProductCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Product, Guid> _productRepository;
    private readonly IUnitOfWork _unitOfWork; // SQL-SERVER-STRATEGY-2
    private readonly ApplicationDbContext _context; // SQL-SERVER-STRATEGY-1

    public UpdateProductCommandHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }
    public async Task<Result> Handle(Command.UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id) ?? throw new ProductException.ProductNotFoundException(request.Id); // Solution 1
        product.Update(request.Name, request.Price, request.Description);
        _unitOfWork.SaveChangesAsync();
        

        return Result.Success();
    }
}
