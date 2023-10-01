import { AnimalType } from "./index"

export interface MapPoint {
  id: string
  report_type: "reported"
  lng: string
  lat: string
  label: string
  createdAt: number
  animaL_type: AnimalType
  breed: string
  animal_type: AnimalType
}
