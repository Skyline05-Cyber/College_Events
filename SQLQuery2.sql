USE CollegeEventHubDb;
GO

INSERT INTO dbo.Events
(
    Title,
    Description,
    Venue,
    ImageUrl,
    StartDate,
    EndDate,
    RegistrationOpensAt,
    RegistrationClosesAt,
    RegistrationFee,
    MaxParticipants,
    IsTeamEvent,
    MinTeamSize,
    MaxTeamSize,
    EligibilityCriteria,
    Rules,
    Status,
    CategoryId,
    OrganizerId
)
VALUES
(
    'College Hackathon 2026',
    'Join our college hackathon to build innovative software projects and compete with fellow students.',
    'College Main Auditorium',
    NULL,
    '2026-11-15T09:00:00',
    '2026-11-15T17:00:00',
    '2026-10-29T00:00:00',
    '2026-11-14T23:59:00',
    0,
    100,
    0,
    1,
    1,
    'Open to college students',
    'Participants must follow college event guidelines.',
    1,
    1,
    NULL
);
GO

SELECT *
FROM dbo.Events;