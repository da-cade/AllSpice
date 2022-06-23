import { AppState } from "../AppState";
import { api } from "./AxiosService";

class IngredientsService {
  async getIngredients(recipeId) {
    const res = await api.get(`api/recipes/${recipeId}/ingredients`)
    AppState.ingredients = res.data
  }
  async makeEdits() {
    const updates = AppState.ingEdits
    const res = await api.put("api/ingredients", updates)
    console.log(res.data)
    AppState.ingEdits = []
    AppState.ingredients = AppState.ingredients
  }
  async createIngredient(ingData) {
    const res = await api.post('api/ingredients', ingData)
    console.log(res.data)
    AppState.ingredients.push(res.data)
  }
  async deleteIngredient(id) {
    const res = await api.delete(`api/ingredients/${id}`)
    console.log(res.data)
    AppState.ingredients = AppState.ingredients.filter(s => s.id !== id)
  }
}

export const ingredientsService = new IngredientsService();