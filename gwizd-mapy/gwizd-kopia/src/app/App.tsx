import { useQuery } from "react-query"
import { GoogleMaps } from "../components"
import { MapPoint } from "../types"
import "./index.css"

function App() {
  const { isLoading, data } = useQuery("reports", () =>
    fetch(
      "https://mezvs42ny3.execute-api.eu-central-1.amazonaws.com/default/events",
      {
        headers: { "Content-Type": "application/json" },
      }
    ).then((data) => data.json())
  )

  return (
    <div className="container">
      {isLoading ? (
        <>Ładowanie danych...</>
      ) : (
        <GoogleMaps points={(data.body.Items as MapPoint[]) || []} zoom={14} />
      )}
    </div>
  )
}

export default App
