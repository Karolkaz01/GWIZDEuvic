import { GoogleMap, MarkerF, useJsApiLoader } from "@react-google-maps/api"
import { useState } from "react"
import { MapPoint, AnimalType } from "../types"
import "./styles.css"
import InfoWindow from "./InfoWindow"

const containerStyle = {
  width: "100%",
  height: "100%",
}

interface GoogleMapsProps {
  points: MapPoint[]
  zoom?: number
}

const center = { lat: 50.0589771, lng: 19.9333073 }

const GoogleMaps = ({ points, zoom }: GoogleMapsProps) => {
  const { isLoaded } = useJsApiLoader({
    id: "google-map-script",
    googleMapsApiKey: import.meta.env.VITE_GOOGLE_PASS,
  })

  const [selectedPoint, setSelectedPoint] = useState<MapPoint | null>(null)

  const onClickOnMarker = (index: number) => {
    if (points[index].id === selectedPoint?.id) setSelectedPoint(null)
    setSelectedPoint(points[index])
  }

  const getIcon = (type: AnimalType) => {
    if (type === "domowy" || type === "domestic") return { url: "domestic.png" }
    if (type === "dziki" || type === "wild") return { url: "wild.png" }
  }

  return isLoaded ? (
    <GoogleMap mapContainerStyle={containerStyle} center={center} zoom={zoom}>
      <>
        {points.length &&
          points.map((point, index) => (
            <MarkerF
              icon={getIcon(point?.animal_type || point.animaL_type)}
              key={`marker-${index}`}
              position={{ lat: Number(point.lat), lng: Number(point.lng) }}
              onClick={() => onClickOnMarker(index)}
            />
          ))}
        {selectedPoint && (
          <InfoWindow
            selectedPoint={selectedPoint}
            setSelectedPoint={setSelectedPoint}
          />
        )}
      </>
    </GoogleMap>
  ) : (
    <>Ładowanie mapy...</>
  )
}

export default GoogleMaps
