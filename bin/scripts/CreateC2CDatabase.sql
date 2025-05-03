CREATE DATABASE IF NOT EXISTS CurrentForCarbon;
USE CurrentForCarbon;
CREATE TABLE PlayerConfidential(
Idx INT NOT NULL AUTO_INCREMENT,
PlayerName VARCHAR(255) NOT NULL,
PlayerID VARCHAR(255) NOT NULL,
playerNumber  VARCHAR(255) NOT NULL,
playerStreet   VARCHAR(255) NOT NULL,
playerCity  VARCHAR(255) NOT NULL,
PlayerState VARCHAR(255) NOT NULL,
PlayerZipcode VARCHAR(255) NOT NULL,
PlayerEmail VARCHAR(255) NOT NULL,
PlayerPhone VARCHAR(255) NOT NULL,
PlayerElectricCo VARCHAR(255) NOT NULL,
PlayerClusterName VARCHAR(255) NOT NULL,
PlayerClusterVersion VARCHAR(255) NOT NULL,
PlayerETHAddress VARCHAR(255) NOT NULL,
PlayerETHKey VARCHAR(255) NOT NULL,
PlayerETHContract VARCHAR(255) NOT NULL,
PlayerDataConnectString VARCHAR(255) NOT NULL,
PlayerMQTTUserName VARCHAR(255) NOT NULL,
PlayerMQTTPassword VARCHAR(255) NOT NULL,
PlayerLastGame VARCHAR(255) NOT NULL,
PlayerMaxDeviation  VARCHAR(255) NOT NULL,
PlayerTotalGamesPlayed VARCHAR(255) NOT NULL,
PlayerRating VARCHAR(255) NOT NULL,
PlayerStatus VARCHAR(255) NOT NULL,
PlayerTimeZone VARCHAR(255) NOT NULL,
PRIMARY KEY (Idx),
UNIQUE KEY `ID_Player` (`PlayerID`));
    
CREATE TABLE IF NOT EXISTS EventResults (
    Idx INT NOT NULL AUTO_INCREMENT ,
    PlayerID VARCHAR(255) NOT NULL,
    EventID VARCHAR(255) NOT NULL,
    StartTime VARCHAR(255) NOT NULL,
    Duration VARCHAR(255) NOT NULL,
    PointsPerWatt VARCHAR(255) NOT NULL,
    AveragePowerInWatts VARCHAR(255) NOT NULL,
    BaselineAveragePowerInWatts VARCHAR(255) NOT NULL,
    DeltaAveragePowerInWatts VARCHAR(255) NOT NULL,
    WattPoints VARCHAR(255) NOT NULL,
    AwardValue VARCHAR(255) NOT NULL,
    EventEnergyInKWHR VARCHAR(255) NOT NULL,
    BaselineEnergyInKWHR VARCHAR(255) NOT NULL,
    Slice VARCHAR(255) NOT NULL,
    PRIMARY KEY (Idx));
    #PRIMARY KEY ( PlayerID , EventID));
    
CREATE TABLE IF NOT EXISTS YearToDateAwards (
    Idx INT NOT NULL AUTO_INCREMENT,
	PlayerID VARCHAR(255) NOT NULL,
    DateCalculated VARCHAR(255) NOT NULL,
    YTDYear VARCHAR(255) NOT NULL,
    TotalWatts VARCHAR(255) NOT NULL,
    TotalPoints VARCHAR(255) NOT NULL,
    TotalPercentPoints VARCHAR(255) NOT NULL,
    TotalAwardValue VARCHAR(255) NOT NULL,
    PRIMARY KEY (Idx));
    #PRIMARY KEY ( PlayerID , YTDYear));

    CREATE TABLE IF NOT EXISTS GameAwards (
    Idx INT NOT NULL AUTO_INCREMENT,
    EventID VARCHAR(255) NOT NULL,
    TotalWatts VARCHAR(255) NOT NULL,
    TotalPoints VARCHAR(255) NOT NULL,
    TotalPercentPoints VARCHAR(255) NOT NULL,
    TotalAwardValue VARCHAR(255) NOT NULL,
    PointMin VARCHAR(255) NOT NULL,
    PointMax VARCHAR(255) NOT NULL,
    PointCount VARCHAR(255) NOT NULL,
    PointMean VARCHAR(255) NOT NULL,
    PointStdDev VARCHAR(255) NOT NULL,
    PRIMARY KEY (Idx));
    #PRIMARY KEY ( GameID ));

    
    CREATE TABLE IF NOT EXISTS GamePlayers (
    Idx INT NOT NULL AUTO_INCREMENT,
    PlayerID VARCHAR(255) NOT NULL,
    DataConnectionString VARCHAR(255) NOT NULL,
    PlayerETHAddress VARCHAR(255) NOT NULL,
    PlayerStatus VARCHAR(255) NOT NULL,
    Slice VARCHAR(255) NOT NULL,
    PRIMARY KEY (Idx));
    #PRIMARY KEY ( PlayerID ));
   
   CREATE TABLE IF NOT EXISTS GameEvents (
    Idx INT NOT NULL AUTO_INCREMENT,
    EventID VARCHAR(255) NOT NULL,
    EventName VARCHAR(255) NOT NULL,
    EventType VARCHAR(255) NOT NULL,
    StartTime VARCHAR(255) NOT NULL,
    EndTime VARCHAR(255) NOT NULL,
    Duration VARCHAR(255) NOT NULL,
    DollarPerPoint VARCHAR(255) NOT NULL,
    PointsPerWatt VARCHAR(255) NOT NULL,
    PointsPerPercent VARCHAR(255) NOT NULL,
    Slice VARCHAR(255) NOT NULL,
    PRIMARY KEY (Idx));
    #PRIMARY KEY ( EventID));
    
    CREATE TABLE IF NOT EXISTS GameResults (
    Idx INT NOT NULL AUTO_INCREMENT,
    PlayerID VARCHAR(255) NOT NULL,
    EventID VARCHAR(255) NOT NULL,
    AveragePowerInWatts VARCHAR(255) NOT NULL,
    BaselineAveragePowerInWatts VARCHAR(255) NOT NULL,
    DeltaAveragePowerInWatts VARCHAR(255) NOT NULL,
    PercentPoints VARCHAR(255) NOT NULL,
    WattPoints VARCHAR(255) NOT NULL,
    TotalPointsAwarded VARCHAR(255) NOT NULL,
    AwardValue VARCHAR(255) NOT NULL,
    Slice VARCHAR(255) NOT NULL,
    PRIMARY KEY (Idx));
	#PRIMARY KEY ( PlayerID , EventID));
    
    CREATE VIEW PlayerYTDAwards AS SELECT PlayerName, PlayerPhone, PlayerEmail, DateCalculated, YTDYear, TotalWatts, TotalPoints, TotalPercentPoints, TotalAwardValue FROM PlayerConfidential INNER JOIN YearToDateAwards USING (PlayerID);
    CREATE VIEW GameTotalAwards AS SELECT EventName,  EventType, StartTime, EndTime, Duration, DollarPerPoint ,PointsPerWatt ,PointsPerPercent, TotalPoints, TotalPercentPoints, TotalAwardValue, PointMin, PointMax,PointCount,PointMean, PointStdDev  FROM GameEvents INNER JOIN GameAwards USING (EventID);
    CREATE VIEW PlayerEventResults AS SELECT PlayerName, PlayerPhone, PlayerEmail, PlayerElectricCo, StartTime, Duration, PointsPerWatt, AveragePowerInWatts, BaselineAveragePowerInWatts, DeltaAveragePowerInWatts, WattPoints, AwardValue, EventEnergyInKWHR, BaselineEnergyInKWHR FROM PlayerConfidential INNER JOIN EventResults USING (PlayerID);
    