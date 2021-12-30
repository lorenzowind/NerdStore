﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NS.Catalog.API.Models;
using NS.WebAPI.Core.Controllers;
using NS.WebAPI.Core.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NS.Catalog.API.Controllers
{
    [Authorize]
    public class CatalogController : MainController
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [AllowAnonymous]
        [HttpGet("catalog/products")]
        public async Task<IEnumerable<Product>> Index()
        {
            return await _productRepository.GetAll();
        }

        [ClaimsAuthorize("Catalog", "Read")]
        [HttpGet("catalogo/products/{id}")]
        public async Task<Product> ProductDetails(Guid id)
        {
            return await _productRepository.GetById(id);
        }
    }
}