function OnIceDestroyed(tile)
    World.Current.PlaceItem("Ice", tile.WorldPosition)
end