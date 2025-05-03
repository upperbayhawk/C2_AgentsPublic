USE CurrentForCarbon;
CREATE TABLE EventResults (
    EventResultsIdx INT NOT NULL AUTO_INCREMENT,
    PlayerID VARCHAR(255) NOT NULL,
    EventID VARCHAR(255) NOT NULL,
    StartTime VARCHAR(255) NOT NULL,
    Duration VARCHAR(255) NOT NULL,
    PointsPerWatt VARCHAR(255) NOT NULL,
    AveragePowerInWatts VARCHAR(255) NOT NULL,
    BaselineAveragePowerInWatts  VARCHAR(255) NOT NULL,
    DeltaAveragePowerInWatts  VARCHAR(255) NOT NULL,
    PercentPoints  VARCHAR(255) NOT NULL,
    WattPoints  VARCHAR(255) NOT NULL,
    TotalPointsAwarded  VARCHAR(255) NOT NULL,
    AwardValue  VARCHAR(255) NOT NULL,
    PRIMARY KEY (EventResultsIdx , PlayerID , EventID);
