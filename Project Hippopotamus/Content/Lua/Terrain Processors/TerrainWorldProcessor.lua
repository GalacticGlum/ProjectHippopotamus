-- FIXME

function Lerp(a, b, time)
    return a + (b - a) * time
end

function Generate(worldData)
    heightMap = {}
    groundHeight = worldData.Height * 0.7

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

    for x = 0, worldData.Width do
        heightMap[x] = groundHeight
    end

    for _, curveParameter in pairs(curveParameters) do
        amplitude = worldData.Height * Lerp(curveParameter[1], curveParameter[2], Random.Value())
        frequency = Lerp(curveParameter[3], curveParameter[4], Random.Value()) / 100

        offset = 0
        phase = Random.Value() * worldData.Width
        for x = 0, worldData.Width do
            heightMap[x] = heightMap[x] + amplitude * math.sin(frequency * x - phase) + offset
        end
    end

    -- Do noise!
    for x = 0, worldData.Width do
        if Random.Value() < noiseChance then
            heightMap[x] = heightMap[x] + Lerp(noiseMinimumMagnitude, noiseMaxMagnitude, Random.Value())
        end
    end

    tilePositions = {}
    for x = 0, worldData.Width do
        for y = 0, worldData.Height do
            if worldData.Height - 1 - y <= heightMap[x] then
                tilePositions[{x, y}] = TileType.Grass
            end
        end
    end

    worldData.SetTileTypes(tilePositions)   
end
