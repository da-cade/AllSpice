import { AppState } from "../AppState";
import { api } from "./AxiosService";

class StepsService {
  async getSteps(id) {
    const res = await api.get(`api/recipes/${id}/steps`)
    let recipes = res.data.sort((a, b) => a.position - b.position)
    AppState.steps = recipes
  }
  async makeEdits() {
    const updates = AppState.stepEdits
    const res = await api.put("api/steps", updates)
    console.log(res.data)
    AppState.stepEdits = []
    AppState.steps = AppState.steps
  }
  async createStep(stepData) {
    const res = await api.post('api/steps', stepData)
    console.log(res.data)
    AppState.steps.push(res.data)
  }
  async deleteStep(id) {
    const res = await api.delete(`api/steps/${id}`)
    console.log(res.data)
    AppState.steps = AppState.steps.filter(s => s.id !== id)
  }
}

export const stepsService = new StepsService();