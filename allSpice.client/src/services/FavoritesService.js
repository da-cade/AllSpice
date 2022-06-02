import { AppState } from "../AppState";
import { api } from "./AxiosService";

class FavoritesService {
  async getMyFavorites() {
    const res = await api.get("account/favorites")
    AppState.myFavorites = res.data
  }
  async getFavoritesByRecipe(id) {
    const res = await api.get(`api/recipes/${id}/favorites`)
    AppState.favorites = AppState.favorites.concat(res.data)
  }
  async addFavorite(favData) {
    const res = await api.post("api/favorites", favData)
    AppState.favorites.push(res.data)
    AppState.myFavorites.push(res.data)
  }
  async removeFavorite(id) {
    const res = await api.delete("api/favorites/" + id)
    AppState.favorites = AppState.favorites.filter(f => f.id !== id)
    AppState.myFavorites = AppState.favorites.filter(f => f.id !== id)
    return res.data
  }
}

export const favoritesService = new FavoritesService();