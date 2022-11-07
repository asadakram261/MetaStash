using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Net;
using System.Linq;
using MiniJSON;
using System.Collections.Generic;
using SimpleJSON;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.UI;
using GoShared;


namespace GoMap
{

	public class GOPlaces : MonoBehaviour
	{

		public GOMap goMap;
		public GameObject Sphere;
		public GameObject Cube;
		private Dollar info;
		private GameObject prefab;
		public bool addGOPlaceComponent = false;




		string URL = "https://dashcache.herokuapp.com/users/getDropLocations";
		[HideInInspector] public IDictionary iconsCache = new Dictionary<string, Sprite>();

		// Use this for initialization
		void Awake()
		{

			if (URL == "")
			{
				Debug.LogWarning("API is missing");
				return;
			}

			//register to the GOMap event OnTileLoad
			goMap.OnTileLoad.AddListener((GOTile) =>
			{
				OnLoadTile(GOTile);
			});

		}

		void OnLoadTile(GOTile tile)
		{
			StartCoroutine(NearbySearch(tile));
		}

		IEnumerator NearbySearch(GOTile tile)
		{

			//Center of the map tile
			Coordinates tileCenter = tile.goTile.tileCenter;

			//radius of the request, equals the tile diagonal /2
			float radius = tile.goTile.diagonalLenght / 2;


			//The complete nearby search url, api key is added at the end
			string url = URL + "location=" + tile.goTile.tileCenter.latitude + "," + tile.goTile.tileCenter.longitude;

			//Perform the request
			int ID = 1;
			var req = new UnityWebRequest(URL + "/?id=" + ID, "GET");

			//byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

			//req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);

			req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			req.SetRequestHeader("Content-Type", "application/json");
			yield return req.SendWebRequest();

			//Check for errors
			if (string.IsNullOrEmpty(req.error))
			{
				/*GameObject placesContainer = new GameObject("Places");
				placesContainer.transform.SetParent(tile.transform);*/
				string response = req.downloadHandler.text;
				//Deserialize the json response
				IDictionary deserializedResponse = (IDictionary)Json.Deserialize(response);

				//Debug.Log(string.Format("[GO Places] Tile center: {0} - Request Url {1} - response {2}", tileCenter.toLatLongString(), url, response));

				//That's our list of Places
				//IList results = (IList)deserializedResponse["results"];
				GameObject placesContainer = new GameObject("Spawn");
				placesContainer.transform.SetParent(tile.transform);


				JSONNode itemsData = JSON.Parse(req.downloadHandler.text);
				for (int i = 0; i < itemsData["data"].Count; i++)
				{
					var Data = new List<Locations>
					{
						new Locations
						{
							id = itemsData["data"][i]["id"] , reward_type = itemsData["data"][i]["reward_type"], reward_amount = itemsData["data"][i]["reward_amount"],
							is_active = itemsData["data"][i]["is_active"], longitude = itemsData["data"][i]["longitude"], latitude = itemsData["data"][i]["latitude"],
							time_to_active = itemsData["data"][i]["time_to_active"], created_at = itemsData["data"][i]["created_at"],updated_at = itemsData["data"][i]["updated_at"]
						}

					};


					foreach (Locations p in Data)
					{
						//	Debug.Log("Longitude"+itemsData["data"]["longitude"]);
						//Debug.Log("Latitude"+itemsData["data"]["latitude"]);


						//GameObject placesContainer = new GameObject("Places");
						//placesContainer.transform.SetParent(tile.transform);

						//foreach (Locations result in Data)
						//{

						//string placeID = (string)result["place_id"];
						//string name = (string)result["name"];

						//IDictionary location = (IDictionary)((IDictionary)result["geometry"])["location"];
						//double lat = (double)location["lat"];
						//double lng = (double)location["lng"];
						/*double lat = p.latitude;
						double lng = p.longitude;
						Debug.Log(lat);
						Debug.Log(lng);*/
						//	IDictionary location = (IDictionary)((IDictionary)result["data"])["location"];
						//Create a new coordinate object, with the desired lat lon
						Coordinates coordinates = new Coordinates(p.latitude, p.longitude, 0);

						if (!TileFilter(tile, coordinates))
							continue;
						if (p.reward_type == "Dollar")
						{
							prefab = Cube;

							info = Cube.GetComponent<Dollar>();
							info.id = p.id;
							info.reward_type = p.reward_type;
							info.reward_amount = p.reward_amount;
							info.is_active = p.is_active;
							info.longitude = p.longitude;
							info.latitude = p.latitude;
							info.updated_at = p.updated_at;
							info.created_at = p.created_at;
						}

						else if (p.reward_type == "Bitcoin")
						{
							prefab = Sphere;

							info = Sphere.GetComponent<Dollar>();
							info.id = p.id;
							info.reward_type = p.reward_type;
							info.reward_amount = p.reward_amount;
							info.is_active = p.is_active;
							info.longitude = p.longitude;
							info.latitude = p.latitude;
							info.updated_at = p.updated_at;
							info.created_at = p.created_at;
						}






						//Instantiate your game object
						GameObject place = GameObject.Instantiate(prefab);
						place.SetActive(true);

						//Convert coordinates to position
						Vector3 position = coordinates.convertCoordinateToVector(place.transform.position.y);

						if (goMap.useElevation)
							position = GOMap.AltitudeToPoint(position);

						//Set the position to object
						place.transform.localPosition = position;
						//the parent
						place.transform.SetParent(placesContainer.transform);

						if (addGOPlaceComponent)
						{
							GOPlacesPrefab component = place.AddComponent<GOPlacesPrefab>();
							//component.placeInfo = result;
							component.goPlaces = this;
						}



					}
				}
			}

			bool TileFilter(GOTile tile, Coordinates coordinates)
			{

				Vector2 tileCoordinates = coordinates.tileCoordinates(goMap.zoomLevel);

				if (tile.goTile.tileCoordinates.Equals(tileCoordinates))
					return true;

				//			Debug.LogWarning ("Coordinates outside the tile");
				return false;

			}

		}

	}
}