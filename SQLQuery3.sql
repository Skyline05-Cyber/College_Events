
USE CollegeEventHubDb;
GO

INSERT INTO dbo.Events
(
    Title, Description, Venue, ImageUrl,
    StartDate, EndDate,
    RegistrationOpensAt, RegistrationClosesAt,
    RegistrationFee, MaxParticipants,
    IsTeamEvent, MinTeamSize, MaxTeamSize,
    EligibilityCriteria, Rules, Status, CategoryId, OrganizerId
)
VALUES
(
    'Web Development Workshop',
    'Learn modern web development fundamentals with practical activities.',
    'Computer Lab 1', NULL,
    '2026-11-10T10:00:00', '2026-11-10T13:00:00',
    '2026-10-29T00:00:00', '2026-11-09T23:59:00',
    0, 80, 0, 1, 1,
    'Open to college students', 'Follow lab safety instructions.',
    1, 1, NULL
),
(
    'Cultural Fest 2026',
    'A celebration of music, dance, art, and student talent.',
    'College Main Auditorium', NULL,
    '2026-11-12T09:00:00', '2026-11-12T18:00:00',
    '2026-10-29T00:00:00', '2026-11-11T23:59:00',
    0, 200, 0, 1, 1,
    'Open to college students', 'Respect all performers and participants.',
    1, 2, NULL
),
(
    'Inter-Department Cricket Tournament',
    'Compete with other departments in our college cricket tournament.',
    'College Sports Ground', NULL,
    '2026-11-18T08:00:00', '2026-11-18T17:00:00',
    '2026-10-29T00:00:00', '2026-11-16T23:59:00',
    0, 100, 1, 11, 11,
    'Open to enrolled college students', 'Teams must follow tournament rules.',
    1, 3, NULL
),
(
    'Academic Quiz Challenge',
    'Test your knowledge in science, technology, and general awareness.',
    'Seminar Hall', NULL,
    '2026-11-20T11:00:00', '2026-11-20T14:00:00',
    '2026-10-29T00:00:00', '2026-11-19T23:59:00',
    0, 100, 0, 1, 1,
    'Open to college students', 'No external assistance during the quiz.',
    1, 4, NULL
),
(
    'AI and Innovation Hackathon',
    'Build creative AI-powered solutions to real-world problems.',
    'Innovation Lab', NULL,
    '2026-11-22T09:00:00', '2026-11-22T18:00:00',
    '2026-10-29T00:00:00', '2026-11-21T23:59:00',
    0, 120, 1, 2, 4,
    'Open to college students', 'Teams must submit original work.',
    1, 1, NULL
);
GO

SELECT Id, Title, Status, CategoryId
FROM dbo.Events
ORDER BY Id DESC;
