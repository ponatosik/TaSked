﻿using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record CreateSubjectCommand(Guid UserId, string SubjectName) : IRequest<SubjectDTO>;
