-- FIXME
-- terrainValleyProcessor = require("TerrainValleyProcessor") 

worldData = {}
worldWidth = 0
worldHeight = 0

function BeginProcess(width, height)
    worldWidth = width
    worldHeight = height
    Generate()
    -- terrainValleyProcessor.Generate()

    Logger.Log("TerrainWorldProcessor", "I WORK!")
end

function Lerp(a, b, time)
    return a + (b - a) * time
end

function Generate()
    local heightMap = {}
    groundHeight = worldHeight * 0.7

    -- { MinimumAmplitude, MaximumAmplitude, MinimumFrequency, MaximumFrequency }
    curveParameters = 
    {
        { 0, 0.3, 0.5, 1.5 }, 
        { 0, 0.01, 5.0, 12.0 },
        { 0, 0.01, 5.0, 12.0 }
    }

    noiseChance = 0.05
    noiseMinimumMagnitude = -2
    noiseMaxMagnitude = 1

    for x = 0, worldWidth do
        heightMap[x] = groundHeight
    end

    for _, curveParameter in pairs(curveParameters) do
        amplitude = worldHeight * Lerp(curveParameter[1], curveParameter[2], Random.Value())
        frequency = Lerp(curveParameter[3], curveParameter[4], Random.Value()) / 100

        offset = 0
        phase = Random.Value() * worldWidth
        for x = 0, worldWidth do
            heightMap[x] = heightMap[x] + amplitude * math.sin(frequency * x - phase) + offset
        end
    end

    -- Do noise!
    for x = 0, worldWidth do
        if Random.Value() < noiseChance then
            heightMap[x] = heightMap[x] + Lerp(noiseMinimumMagnitude, noiseMaxMagnitude, Random.Value())
        end
    end

    for x = 0, worldWidth do
        for y = 0, worldHeight do
            if worldHeight - 1 - y <= heightMap[x] then
                worldData[x * worldHeight + y] = true--TileType.Grass
            end
        end
    end
end
