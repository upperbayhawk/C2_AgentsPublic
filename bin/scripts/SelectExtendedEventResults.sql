SELECT 
        p.PlayerName ,
        e.EventType,
        e.EventName,
        er.StartTime,
        er.Duration,
        er.PointsPerWatt,
        er.AveragePowerInWatts,
        er.BaselineAveragePowerInWatts,
        er.DeltaAveragePowerInWatts,
        er.WattPoints,
        er.AwardValue,
        er.EventEnergyInKWHR,
        er.BaselineEnergyInKWHR
    FROM
        CurrentForCarbon.GameEvents e,
        CurrentForCarbon.PlayerConfidential p,
        CurrentForCarbon.EventResults er
	WHERE
        p.PlayerId = er.PlayerId and e.EventId = er.EventId
        ORDER by e.Idx