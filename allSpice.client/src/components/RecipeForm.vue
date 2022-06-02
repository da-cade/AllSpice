<template>
  <div class="recipeForm">
    <form @submit.prevent="createRecipe()">
      <div class="container-fluid">
        <div class="row">
          <div class="col-md-6">
            <label for="recipe-title">Title: </label>
            <input
              type="text"
              v-model="formData.title"
              name="recipe-title"
              id=""
              required
              maxlength="50"
            />
          </div>
          <div class="col-md-6">
            <label for="recipe-img">Image: </label>
            <input
              type="text"
              v-model="formData.picture"
              name="recipe-img"
              id=""
              required
            />
          </div>
        </div>
        <div class="row d-flex align-items-end">
          <div class="col-md-6">
            <label for="recipe-subtitle">Subtitle: </label>
            <input
              type="text"
              v-model="formData.subtitle"
              name="recipe-subtitle"
              id=""
              required
              maxlength="255"
            />
          </div>
          <div class="col-md-6 d-flex justify-content-center">
            <div class="btn-group-vertical w-100" role="group" aria-label="">
              <div class="btn-group" role="group">
                <button
                  id="dropdownId"
                  type="button"
                  class="btn btn-light dropdown-toggle"
                  data-bs-toggle="dropdown"
                  aria-haspopup="true"
                  aria-expanded="false"
                >
                  {{ formData.category ? formData.category : "Category" }}
                </button>
                <div class="dropdown-menu" aria-labelledby="dropdownId">
                  <a
                    v-for="c in categories"
                    :key="c"
                    class="dropdown-item selectable"
                    @click="formData.category = c"
                    >{{ c }}</a
                  >
                </div>
              </div>
            </div>
          </div>
        </div>
        <hr />
        <div class="row">
          <div class="col-12">
            <div class="w-100 d-flex justify-content-start">
              <button class="btn btn-info me-2" type="submit">Submit</button>
              <button class="btn btn-light" @click="formData = {}">
                Close
              </button>
            </div>
          </div>
        </div>
      </div>
    </form>
  </div>
</template>


<script>
import { ref } from "@vue/reactivity"
import { AppState } from "../AppState"
import { onMounted, watchEffect } from "@vue/runtime-core"
import { recipesService } from "../services/RecipesService"
import Pop from "../utils/Pop"
import { logger } from "../utils/Logger"
export default {
  setup() {
    const formData = ref({})
    // TODO take this out when migrating back to single page
    onMounted(async () => {
      await recipesService.getAllRecipes()
    })
    let categories = ref([])
    watchEffect(() => {
      if (AppState.recipes.length) {
        let arr = []
        AppState.recipes.forEach(r => {
          if (!arr.includes(r.category)) {
            arr.push(r.category)
          }
        })
        arr.sort()
        categories.value = arr
      }
    })
    return {
      formData,
      categories,
      async createRecipe() {
        try {
          if (!formData.value.category) {
            throw new Error("You have to choose a category")
          }
          await recipesService.createRecipe(formData.value)
          formData.value = {}
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      }
    }
  }
}
</script>


<style lang="scss" scoped>
</style>