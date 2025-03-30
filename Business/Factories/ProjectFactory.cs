using Data.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Business.Factories
{
    public static class ProjectFactory
    {
        public static Project Map(ProjectEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            var project = new Project
            {
                Id = entity.Id,
                Image = entity.Image,
                ProjectName = entity.ProjectName,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Budget = entity.Budget,
                UserId = entity.UserId,
                Client = new Client
                {
                    Id = entity.Client.Id,
                    ClientName = entity.Client.ClientName
                },

                Status = new Status
                {
                    Id = entity.Status.Id,
                    StatusName = entity.Status.StatusName
                }
            };

            return project;
        }

        public static ProjectEntity Map(ProjectRegistrationForm form)
        {
            if (form == null)
            {
                return null;
            }
            var projectEntity = new ProjectEntity
            {
                Image = form.Image,
                ProjectName = form.ProjectName,
                Description = form.Description,
                StartDate = form.StartDate,
                EndDate = form.EndDate,
                // Project owner
                UserId = form.UserId,
                Created = DateTime.UtcNow,
                Budget = form.Budget,
                ClientId = form.ClientId,
                // Sätter projektet till Active
                StatusId = 1
            };

            return projectEntity;
        }

        public static ProjectEntity Map(ProjectUpdateForm form, ProjectEntity entity)
        {
            if (form == null || entity == null)
            {
                return null;
            }

            entity.Id = form.Id;
            entity.ClientId = form.ClientId;
            entity.StatusId = form.StatusId;
            entity.Image = form.Image;
            entity.ProjectName = form.ProjectName;
            entity.Description = form.Description;
            entity.StartDate = form.StartDate;
            entity.EndDate = form.EndDate;
            entity.Budget = form.Budget;
            entity.UserId = form.UserId;

            return entity;
        }
    }
}