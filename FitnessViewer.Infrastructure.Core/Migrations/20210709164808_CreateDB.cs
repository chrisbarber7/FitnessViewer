using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessViewer.Infrastructure.Core.Migrations
{
    public partial class CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRide = table.Column<bool>(type: "bit", nullable: false),
                    IsRun = table.Column<bool>(type: "bit", nullable: false),
                    IsSwim = table.Column<bool>(type: "bit", nullable: false),
                    IsOther = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Calendar",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    YearMonth = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    YearWeek = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    MonthName = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    DayName = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    WeekLabel = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendar", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "PeakStreamTypeDuration",
                columns: table => new
                {
                    PeakStreamType = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeakStreamTypeDuration", x => new { x.PeakStreamType, x.Duration });
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AthleteSetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    DashboardStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DashboardEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DashboardRange = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ShowRun = table.Column<bool>(type: "bit", nullable: false),
                    ShowRide = table.Column<bool>(type: "bit", nullable: false),
                    ShowSwim = table.Column<bool>(type: "bit", nullable: false),
                    ShowOther = table.Column<bool>(type: "bit", nullable: false),
                    ShowAll = table.Column<bool>(type: "bit", nullable: false),
                    RunDistanceUnit = table.Column<int>(type: "int", nullable: false),
                    RideDistanceUnit = table.Column<int>(type: "int", nullable: false),
                    SwimDistanceUnit = table.Column<int>(type: "int", nullable: false),
                    OtherDistanceUnit = table.Column<int>(type: "int", nullable: false),
                    AllDistanceUnit = table.Column<int>(type: "int", nullable: false),
                    RunPaceUnit = table.Column<int>(type: "int", nullable: false),
                    RidePaceUnit = table.Column<int>(type: "int", nullable: false),
                    SwimPaceUnit = table.Column<int>(type: "int", nullable: false),
                    OtherPaceUnit = table.Column<int>(type: "int", nullable: false),
                    AllPaceUnit = table.Column<int>(type: "int", nullable: false),
                    RunElevationUnit = table.Column<int>(type: "int", nullable: false),
                    RideElevationUnit = table.Column<int>(type: "int", nullable: false),
                    SwimElevationUnit = table.Column<int>(type: "int", nullable: false),
                    OtherElevationUnit = table.Column<int>(type: "int", nullable: false),
                    AllElevationUnit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthleteSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AthleteSetting_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Metric",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    MetricType = table.Column<int>(type: "int", nullable: false),
                    Recorded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsManual = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metric", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metric_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Queue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Added = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Processed = table.Column<bool>(type: "bit", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivityId = table.Column<long>(type: "bigint", nullable: true),
                    HasError = table.Column<bool>(type: "bit", nullable: true),
                    DownloadType = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Queue_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ZoneType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zone_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZoneRange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ZoneType = table.Column<int>(type: "int", nullable: false),
                    ZoneName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ZoneStart = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneRange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneRange_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Athlete",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    AthleteSettingId = table.Column<long>(type: "bigint", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileMedium = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Friend = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Follower = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApproveFollowers = table.Column<bool>(type: "bit", nullable: false),
                    AthleteType = table.Column<int>(type: "int", nullable: false),
                    DatePreference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementPreference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FTP = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresAt = table.Column<int>(type: "int", nullable: false),
                    ExpiresIn = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Athlete", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Athlete_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Athlete_AthleteSetting_AthleteSettingId",
                        column: x => x.AthleteSettingId,
                        principalTable: "AthleteSetting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    AthleteId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SufferScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmbedToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Distance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DistanceInMiles = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPhotoCount = table.Column<int>(type: "int", nullable: false),
                    ElevationGain = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HasKudoed = table.Column<bool>(type: "bit", nullable: false),
                    AverageHeartrate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxHeartrate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Truncated = table.Column<int>(type: "int", nullable: true),
                    GearId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageSpeed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxSpeed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageCadence = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageTemperature = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AveragePower = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kilojoules = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsTrainer = table.Column<bool>(type: "bit", nullable: false),
                    IsCommute = table.Column<bool>(type: "bit", nullable: false),
                    IsManual = table.Column<bool>(type: "bit", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    IsFlagged = table.Column<bool>(type: "bit", nullable: false),
                    AchievementCount = table.Column<int>(type: "int", nullable: false),
                    KudosCount = table.Column<int>(type: "int", nullable: false),
                    CommentCount = table.Column<int>(type: "int", nullable: false),
                    AthleteCount = table.Column<int>(type: "int", nullable: false),
                    PhotoCount = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDateLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovingTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ElapsedTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartLatitude = table.Column<double>(type: "float", nullable: true),
                    StartLongitude = table.Column<double>(type: "float", nullable: true),
                    WeightedAverageWatts = table.Column<int>(type: "int", nullable: false),
                    EndLatitude = table.Column<double>(type: "float", nullable: true),
                    EndLongitude = table.Column<double>(type: "float", nullable: true),
                    HasPowerMeter = table.Column<bool>(type: "bit", nullable: false),
                    MapId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MapPolyline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MapPolylineSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Calories = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StreamSize = table.Column<int>(type: "int", nullable: true),
                    StreamStep = table.Column<int>(type: "int", nullable: true),
                    DetailsDownloaded = table.Column<bool>(type: "bit", nullable: false),
                    TSS = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IntensityFactor = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_ActivityType_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "ActivityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activity_Athlete_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "Athlete",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activity_Calendar_Start",
                        column: x => x.Start,
                        principalTable: "Calendar",
                        principalColumn: "Date",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gear",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AthleteId = table.Column<long>(type: "bigint", nullable: false),
                    GearType = table.Column<int>(type: "int", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrameType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Distance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ResourceState = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gear", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gear_Athlete_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "Athlete",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityPeak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    StreamType = table.Column<int>(type: "int", nullable: false),
                    Peak5 = table.Column<int>(type: "int", nullable: true),
                    Peak10 = table.Column<int>(type: "int", nullable: true),
                    Peak30 = table.Column<int>(type: "int", nullable: true),
                    Peak60 = table.Column<int>(type: "int", nullable: true),
                    Peak120 = table.Column<int>(type: "int", nullable: true),
                    Peak300 = table.Column<int>(type: "int", nullable: true),
                    Peak360 = table.Column<int>(type: "int", nullable: true),
                    Peak600 = table.Column<int>(type: "int", nullable: true),
                    Peak720 = table.Column<int>(type: "int", nullable: true),
                    Peak1200 = table.Column<int>(type: "int", nullable: true),
                    Peak1800 = table.Column<int>(type: "int", nullable: true),
                    Peak3600 = table.Column<int>(type: "int", nullable: true),
                    PeakDuration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityPeak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityPeak_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityPeakDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: true),
                    StartIndex = table.Column<int>(type: "int", nullable: true),
                    EndIndex = table.Column<int>(type: "int", nullable: true),
                    StreamType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityPeakDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityPeakDetail_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BestEffort",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    ResourceState = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ElapsedTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    MovingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDateLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Distance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartIndex = table.Column<int>(type: "int", nullable: false),
                    EndIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BestEffort", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BestEffort_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lap",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    ResourceState = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ElapsedTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Distance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartIndex = table.Column<int>(type: "int", nullable: false),
                    EndIndex = table.Column<int>(type: "int", nullable: false),
                    TotalElevationGain = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageSpeed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxSpeed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageCadence = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AveragePower = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageHeartrate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxHeartrate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LapIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lap_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<long>(type: "bigint", nullable: true),
                    ItemsAdded = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stream",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Distance = table.Column<double>(type: "float", nullable: true),
                    Altitude = table.Column<double>(type: "float", nullable: true),
                    Velocity = table.Column<double>(type: "float", nullable: true),
                    HeartRate = table.Column<int>(type: "int", nullable: true),
                    Cadence = table.Column<int>(type: "int", nullable: true),
                    Watts = table.Column<int>(type: "int", nullable: true),
                    Temperature = table.Column<int>(type: "int", nullable: true),
                    Moving = table.Column<bool>(type: "bit", nullable: true),
                    Gradient = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stream", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stream_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserNotification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotification_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNotification_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ActivityTypeId",
                table: "Activity",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_AthleteId",
                table: "Activity",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_Start",
                table: "Activity",
                column: "Start");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPeak_ActivityId",
                table: "ActivityPeak",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPeakDetail_ActivityId",
                table: "ActivityPeakDetail",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPeakDetail_DurationAndStreamType",
                table: "ActivityPeakDetail",
                columns: new[] { "Duration", "StreamType" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPeakDetail_Value",
                table: "ActivityPeakDetail",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Athlete_AthleteSettingId",
                table: "Athlete",
                column: "AthleteSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Athlete_UserId",
                table: "Athlete",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AthleteSetting_UserId",
                table: "AthleteSetting",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BestEffort_ActivityId",
                table: "BestEffort",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_BestEffort_ElapsedTime",
                table: "BestEffort",
                column: "ElapsedTime");

            migrationBuilder.CreateIndex(
                name: "IX_BestEffort_Name",
                table: "BestEffort",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Gear_AthleteId",
                table: "Gear",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_Lap_ActivityId",
                table: "Lap",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Metric_UserIdRecorded",
                table: "Metric",
                columns: new[] { "UserId", "Recorded", "MetricType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ActivityId",
                table: "Notification",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Queue_UserId",
                table: "Queue",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stream_ActivityId",
                table: "Stream",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stream_ActivityIdAndStream",
                table: "Stream",
                columns: new[] { "ActivityId", "Time" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserNotification_NotificationId",
                table: "UserNotification",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotification_UserId",
                table: "UserNotification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_UserId",
                table: "Zone",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneRange_UserId",
                table: "ZoneRange",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityPeak");

            migrationBuilder.DropTable(
                name: "ActivityPeakDetail");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BestEffort");

            migrationBuilder.DropTable(
                name: "Gear");

            migrationBuilder.DropTable(
                name: "Lap");

            migrationBuilder.DropTable(
                name: "Metric");

            migrationBuilder.DropTable(
                name: "PeakStreamTypeDuration");

            migrationBuilder.DropTable(
                name: "Queue");

            migrationBuilder.DropTable(
                name: "Stream");

            migrationBuilder.DropTable(
                name: "UserNotification");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropTable(
                name: "ZoneRange");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "ActivityType");

            migrationBuilder.DropTable(
                name: "Athlete");

            migrationBuilder.DropTable(
                name: "Calendar");

            migrationBuilder.DropTable(
                name: "AthleteSetting");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
