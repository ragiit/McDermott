global using Mapster;
global using McDermott.Application.Dtos;
global using McDermott.Application.Interfaces.Repositories;
global using McDermott.Domain.Entities;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using System.ComponentModel.DataAnnotations;

global using McDermott.Application.Dtos.Medical;
global using McDermott.Application.Dtos.Config;

global using static McDermott.Application.Features.Commands.Medical.DoctorScheduleCommand;
global using static McDermott.Application.Features.Commands.Config.GenderCommand;
global using static McDermott.Application.Features.Commands.Config.GroupCommand;
global using static McDermott.Application.Features.Commands.Medical.HealthCenterCommand;
global using static McDermott.Application.Features.Commands.Medical.BuildingCommand;
global using static McDermott.Application.Features.Commands.Medical.SpecialityCommand;
global using static McDermott.Application.Features.Commands.Config.ReligionCommand;