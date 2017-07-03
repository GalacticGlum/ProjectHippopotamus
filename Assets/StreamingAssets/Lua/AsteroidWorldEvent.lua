function AsteroidEventPrecondition(worldEvent, deltaTime)
    -- worldEvent.Timer = worldEvent.Timer + deltaTime
    -- local timer = worldEvent.Timer
    -- if timer >= 30.0 then
    --     worldEvent.Timer = 0
    --     return true
    -- end

    return true
end

function AsteroidEventAction(worldEvent)
    WorldController.Instance.AsteroidController.SpawnAtPlayer(5, 10)
end