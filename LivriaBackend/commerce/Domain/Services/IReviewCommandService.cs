using LivriaBackend.commerce.Domain.Model.Aggregates;
using LivriaBackend.commerce.Domain.Model.Commands;
using LivriaBackend.commerce.Domain.Model.Entities;
using LivriaBackend.commerce.Domain.Repositories;
using LivriaBackend.shared.Domain.Repositories;
using LivriaBackend.users.Domain.Model.Repositories;
using System;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Domain.Model.Services
{
    public interface IReviewCommandService
    {
        Task<Review> Handle(CreateReviewCommand command);
        Task<Review> Handle(UpdateReviewCommand command);
        Task<bool> Handle(DeleteReviewCommand command);
    }
}