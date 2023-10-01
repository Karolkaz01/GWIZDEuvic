import { InfoWindowF } from "@react-google-maps/api"
import { MapPoint } from "../types"
import { Dispatch, SetStateAction } from "react"

interface InfoWindowProps {
  selectedPoint: MapPoint
  setSelectedPoint: Dispatch<SetStateAction<MapPoint | null>>
}

const InfoWindow = ({ selectedPoint, setSelectedPoint }: InfoWindowProps) => {
  const { animaL_type, breed, label, lat, lng } = selectedPoint

  const hasData = Boolean(breed || label)

  return (
    <InfoWindowF
      position={{
        lat: Number(lat),
        lng: Number(lng),
      }}
      onCloseClick={() => setSelectedPoint(null)}
    >
      <div className="info-box">
        <p className="title">
          Zgłoszenie (
          {animaL_type === "domestic" || animaL_type === "domowy"
            ? "Domowe"
            : "Dzikie"}{" "}
          zwierze)
        </p>
        <div className="description">
          {hasData ? (
            <>
              {label && (
                <p>
                  <strong>Typ:</strong> {label}
                </p>
              )}
              {breed && (
                <p>
                  <strong>Rasa:</strong> {breed}
                </p>
              )}
            </>
          ) : (
            <>Brak danych</>
          )}
        </div>
      </div>
    </InfoWindowF>
  )
}

export default InfoWindow
