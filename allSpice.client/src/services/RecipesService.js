import { AppState } from "../AppState";
import { logger } from "../utils/Logger";
import { api } from "./AxiosService";

class RecipesService {
  async getAllRecipes() {
    const res = await api.get('api/recipes')
    AppState.recipes = res.data
  }
  async createRecipe(formData) {
    const res = await api.post('api/recipes', formData)
    console.log(res.data)
    AppState.recipes.unshift(res.data)
  }
  async deleteRecipe(id) {
    const res = await api.delete(`api/recipes/${id}`)
    console.log(res.data)
    AppState.recipes = AppState.recipes.filter(r => r.id !== id)
  }
}

export const recipesService = new RecipesService();