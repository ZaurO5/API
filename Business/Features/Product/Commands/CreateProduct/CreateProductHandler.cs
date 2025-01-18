﻿using AutoMapper;
using Business.Wrappers;
using Common.Exceptions;
using Data.Repositories.Product;
using Data.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Product.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IMapper _mapper;
        public CreateProductHandler(IUnitOfWork unitOfWork,
            IProductWriteRepository productRepository,
            IProductReadRepository productReadRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productWriteRepository = productRepository;
            _productReadRepository = productReadRepository;
            _mapper = mapper;
        }
        public async Task<Response> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var result = await new CreateProductCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var product = await _productReadRepository.GetByNameAsync(request.Name);
            if (product is not null)
                throw new ValidationException("The product with this name is already exist");

            product = _mapper.Map<Common.Entities.Product>(request);

            await _productWriteRepository.CreateAsync(product);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Product created successfully"
            };
        }
    }
}
