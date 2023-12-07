﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Contract.Abstractions.Message;
using BE.Contract.Abstractions.Shared;
using BE.Domain.Abstractions.Repositories;
using BE.Domain.Abstractions;
using BE.Persistance;
using MediatR;
using BE.Contract.Services.Product;

namespace BE.Application.UserCases.V1.Commands.Order;
public sealed class CreateProductCommandHandler : ICommandHandler<Command.CreateProductCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Product, Guid> _productRepository;
    private readonly IUnitOfWork _unitOfWork; // SQL-SERVER-STRATEGY-2
    private readonly ApplicationDbContext _context; // SQL-SERVER-STRATEGY-1
    private readonly IPublisher _publisher;

    public CreateProductCommandHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepository,
        IUnitOfWork unitOfWork,
        IPublisher publisher,
        ApplicationDbContext context)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _context = context;
        _publisher = publisher;
    }

    public async Task<Result> Handle(Command.CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Domain.Entities.Product.CreateProduct(Guid.NewGuid(), request.Name, request.Price, request.Description);

        _productRepository.Add(product);
        //await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _context.SaveChangesAsync();

        // Try to get product ID
        /*  var productCreated = await _productRepository.FindByIdAsync(product.Id);

          var productSecond = Domain.Entities.Product.CreateProduct(Guid.NewGuid(), productCreated.Name + " Second",
              productCreated.Price,
              productCreated.Id.ToString());

          _productRepository.Add(productSecond);
          //await _unitOfWork.SaveChangesAsync(cancellationToken);
          await _context.SaveChangesAsync();
        */
        // => Send Email
        //await _publisher.Publish(new DomainEvent.ProductCreated(productCreated.Id), cancellationToken);
        //await _publisher.Publish(new DomainEvent.ProductDeleted(product.Id), cancellationToken);

        // await Task.WhenAll(
        //          _publisher.Publish(new DomainEvent.ProductCreated(productCreated.Id), cancellationToken),
        //          _publisher.Publish(new DomainEvent.ProductDeleted(product.Id), cancellationToken));

        return Result.Success();
    }
}
