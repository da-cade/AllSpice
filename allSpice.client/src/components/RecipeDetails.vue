<template>
  <div
    class="detailsContent rounded"
    :style="{ 'background-image': `url(${recipe.picture})` }"
  >
    <div class="coverImg rounded-top">
      <div class="buttonBox d-flex flex-column align-items-center">
        <i
          id="close"
          class="mdi mdi-close selectable text-center rounded-circle mb-2"
          @click.stop="closeModal"
        ></i>
        <i
          class="fav mdi mdi-heart selectable"
          @click.stop="toggleFavorite"
          v-if="favorited"
        ></i>
        <i
          class="fav mdi mdi-heart-outline selectable"
          @click.stop="toggleFavorite"
          v-if="!favorited && account.id"
        ></i>
      </div>
    </div>

    <div class="body bg-light">
      <div class="d-flex justify-content-between">
        <div class="w-65 p-4">
          <h1>{{ recipe.title }}</h1>

          <hr class="w-35" />
          <h4>{{ recipe.subtitle }}</h4>
        </div>
        <div class="d-flex w-35 p-4">
          <!-- <h5>{{ recipe.author }}</h5> -->
          <!-- TODO category is a router-link to search by that category -->
          <h5 class="ms-auto">{{ recipe.category }}</h5>
        </div>
      </div>
      <!-- <button class="m-4">Save this recipe</button> -->
    </div>
    <div class="container-fluid">
      <div class="row">
        <div class="col-12">
          <div class="whitespacer"></div>
        </div>
        <div class="col-12 d-flex justify-content-end">
          <div class="d-flex lighten rounded-circle p-1">
            <i
              v-if="authenticated"
              class="mdi mdi-pencil selectable rounded-circle p-1"
              @click.stop="handleEdit"
            ></i>
          </div>
        </div>
      </div>

      <hr />
      <div class="row">
        <div class="col-md-5 bg-myLitePink">
          <div class="bg-light rounded p-2 my-3">
            <h4>Ingredients</h4>
            <!-- REVIEW @updateMe="handleEdit(payload)" -->
            <Ingredient
              class=""
              v-for="i in ingredients"
              :key="i.id"
              :ingredient="i"
              :editing="editing"
            />
            <i
              class="mdi mdi-plus bg-danger rounded-circle selectable"
              v-if="editing"
              @click.stop="creatingIng = !creatingIng"
            ></i>
            <form @submit.prevent="newIngredient()">
              <div id="newIngredient" class="d-flex">
                <label for=""></label>
                <input
                  type="text"
                  v-if="creatingIng && editing"
                  v-model="ingData.name"
                  required
                />
                <label for=""></label>
                <input
                  type="text"
                  style="width: 5vw"
                  v-if="creatingIng && editing"
                  v-model="ingData.quantity"
                  required
                />
                <button
                  type="submit"
                  v-if="creatingIng && editing"
                  style="display: none"
                />
              </div>
            </form>
          </div>
        </div>
        <div class="col-md-5 bg-myLitePink">
          <div class="bg-light rounded p-2 my-3">
            <h4>Steps</h4>
            <Step
              class=""
              v-for="s in steps"
              :key="s.id"
              :step="s"
              :editing="editing"
            />
            <i
              class="mdi mdi-plus bg-danger rounded-circle h-25 selectable"
              v-if="editing"
              @click.stop="creatingStep = !creatingStep"
            ></i>
            <form @submit.prevent="newStep()">
              <div id="newStep" class="d-flex">
                <label for=""></label>
                <input
                  type="number"
                  min="0"
                  style="width: 3vw"
                  v-if="creatingStep && editing"
                  v-model.trim="stepData.position"
                  required
                />
                <label for=""></label>
                <textarea
                  class="w-100"
                  v-if="creatingStep && editing"
                  v-model.trim="stepData.body"
                  required
                />
                <button
                  type="submit"
                  v-if="creatingStep && editing"
                  style="display: none"
                />
              </div>
            </form>
            <!-- TODO we want to use @submit.prevent="newStep" or something on submit to open up another new input -->
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col-12">
          <div class="whitespacer"></div>
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import { computed, onMounted, ref, watchEffect } from "@vue/runtime-core"
import { AppState } from "../AppState"
import { useRoute, useRouter } from "vue-router"
import { ingredientsService } from "../services/IngredientsService";
import { stepsService } from "../services/StepsService";
import { favoritesService } from "../services/FavoritesService";
import Pop from "../utils/Pop";
import Ingredient from "./Ingredient.vue";
import { logger } from "../utils/Logger";
export default {
  components: { Ingredient },
  inheritAttrs: false,
  name: 'RecipeDetails',
  props: {
    recipe: {
      type: Object,
      required: true
    }
  },
  setup(props) {
    const route = useRoute()
    const router = useRouter()
    const editing = ref(false)
    const creatingIng = ref(false)
    const creatingStep = ref(false)
    const loaded = ref(false)
    const favorited = ref(false)
    const stepData = ref({})
    const ingData = ref({})
    //cdur tosrue
    watchEffect(() => {
      const favorite = AppState.favorites.find(f => (f.accountId == AppState.account.id) && (f.accountId == AppState.account.id))
      if (favorite) {
        favorited.value = true
      }
      if (!favorite) {
        favorited.value = false
      }
    })

    // watchEffect(async () => {
    //   if (editing.value == false && AppState.ingEdits.length) {
    //     editing.value == null
    //     console.log('moved to service')
    //     await ingredientsService.makeEdits()
    //     editing.value = false
    //   }
    // })

    onMounted(async () => {
      await favoritesService.getFavoritesByRecipe(route.params.recipeId)
      await ingredientsService.getIngredients(route.params.recipeId)
      await stepsService.getSteps(route.params.recipeId)
      // TODO refactor loaded with loading animations and waits
      loaded.value = true
    })
    return {
      props, route, router,
      loaded,
      editing, creatingIng, creatingStep,
      stepData, ingData,
      favorited,
      ingredients: computed(() => AppState.ingredients),
      steps: computed(() => AppState.steps),
      account: computed(() => AppState.account),
      authenticated: computed(() => AppState.account.id == props.recipe.creatorId),
      closeModal() {
        try {
          router.go(-1)
        } catch (error) {
          router.push({ name: 'Home' })
        }
      },
      async toggleFavorite() {
        const found = AppState.favorites.find(f => (f.recipeId == props.recipe.id) && (f.accountId == AppState.account.id))
        if (!found) {
          await favoritesService.addFavorite({ recipeId: props.recipe.id })
        } else {
          const message = await favoritesService.removeFavorite(found.id)
          Pop.toast(message, 'success', 'top-end', 2000, false)
        }
      },
      async handleEdit() {
        editing.value = !editing.value
        try {
          if (!AppState.stepEdits.length && !AppState.ingEdits.length) {
            return
          }
          if (editing.value == false && AppState.ingEdits.length) {
            await ingredientsService.makeEdits()
          }
          if (editing.value == false && AppState.stepEdits.length) {
            await stepsService.makeEdits()
          }
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
      async newStep() {
        try {
          stepData.value.recipeId = route.params.recipeId
          stepData.value.creatorId = this.account.id
          console.log(stepData.value)
          await stepsService.createStep(stepData.value)
          stepData.value = {}
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
      async newIngredient() {
        try {
          ingData.value.recipeId = route.params.recipeId
          ingData.value.creatorId = this.account.id
          console.log(ingData.value)
          await ingredientsService.createIngredient(ingData.value)
          ingData.value = {}
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
@import "src/assets/scss/_variables.scss";
.lighten {
  background-color: $light;
}
hr {
  color: $light;
  height: 2px;
}
.body {
  min-height: 25vh;
}
.detailsContent {
  position: relative;
  height: 90vh;
  width: 90vw;
  overflow-y: auto;
  background-size: cover;
  background-repeat: no-repeat;
  background-position: center;
  background-attachment: fixed;
}
.coverImg {
  height: 65vh;
  width: 100%;
  background-size: cover;
  background-repeat: no-repeat;
  background-position: center;
  background-attachment: fixed;
  // z-index: -5;
}
.whitespacer {
  height: 15vh;
}
.ingredientText {
  font-size: 20px;
}
.stepText {
  font-size: 20px;
}
.buttonBox {
  position: fixed;
  top: 11vh;
  right: 6vw;
  font-size: 20pt;
  z-index: 11;
}
.fav {
  text-align: center;
  border-radius: 50%;
  background-color: white;
  height: 2rem !important;
  width: 2rem !important;
  color: rgb(245, 75, 75);
}

#close {
  border-radius: 50%;
  height: 2rem !important;
  width: 2rem !important;
  background-color: rgb(247, 231, 94);
}

input:not(:placeholder-shown):valid {
  border: none;
}
input {
  width: fit-content;
  height: 3vh;
  margin: 0.25rem;
  border: none;
  overflow-y: hidden;
  border-radius: 3px;
}
input:focus {
  outline: none;
}
</style>