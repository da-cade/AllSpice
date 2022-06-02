<template>
  <div class="col-md-6">
    <!-- TODO change router-link to method -->
    <!-- TODO on hover car -->
    <div
      class="spicebox d-flex flex-column rounded my-4"
      @click.stop="goToDetails()"
    >
      <img
        :src="recipe.picture"
        :alt="recipe.title"
        class="spicePic rounded-top"
      />
      <div class="spiceContent d-flex flex-column p-3">
        <h5>{{ recipe.title }}</h5>
        <h6>{{ recipe.subtitle }}</h6>
        <div class="mt-auto">
          <hr />
          <div class="d-flex justify-content-between">
            <span>{{ recipe.creator.name }}</span>
            <!-- TODO some nonsense here -->
            <div
              class="
                w-50
                spiceInfo
                d-flex
                justify-content-end
                align-items-center
              "
            >
              <span class="me-2">{{ recipe.category }}</span>
              <div v-if="account.id" class="me-2">
                <i
                  class="mdi mdi-heart selectable"
                  style="color: rgb(191, 17, 17)"
                  @click.stop="toggleFavorite"
                  v-if="favorited"
                ></i>
                <i
                  class="mdi mdi-heart-outline selectable"
                  @click.stop="toggleFavorite"
                  v-if="!favorited"
                ></i>
              </div>
              <i
                v-if="authenticated"
                class="mdi mdi-delete selectable"
                @click.stop="deleteRecipe"
              ></i>
            </div>
            <div class="d-flex justify-content-between mb-1"></div>
          </div>
        </div>
      </div>
    </div>
  </div>

  <Transition name="modalFade">
    <keep-alive>
      <Modal v-if="showModal">
        <template #modal-content-slot>
          <router-view :recipe="recipe" />
        </template>
      </Modal>
    </keep-alive>
  </Transition>
</template>


<script>
import { computed, onMounted, ref, watchEffect } from "@vue/runtime-core";
import { useRoute, useRouter } from "vue-router"
import { AppState } from "../AppState";

import { logger } from "../utils/Logger";
import Pop from "../utils/Pop";
import { recipesService } from "../services/RecipesService";
import { favoritesService } from "../services/FavoritesService";
export default {
  props: {
    recipe: {
      type: Object,
      required: true
    }
  },
  setup(props) {
    const route = useRoute();
    const router = useRouter();

    onMounted(async () => {
      try {
        await favoritesService.getFavoritesByRecipe(props.recipe.id)
      } catch (error) {
        logger.error(error)
        Pop.toast(error.message, 'error')
      }
    })
    // REVIEW could this be abstracted to global scope?
    let favorited = ref(false)
    watchEffect(() => {
      if (AppState.recipes.length && AppState.favorites.length) {
        const favorite = AppState.favorites.find(f => (f.recipeId == props.recipe.id) && (f.accountId == AppState.account.id))
        if (favorite) {
          favorited.value = true
        }
        if (!favorite) {
          favorited.value = false
        }
      }
    })

    let showModal = ref()
    watchEffect(() => {
      if (route.params.recipeId == props.recipe.id) {
        showModal.value = route.meta.showModal
      }
      if (route.name == "Home") {
        showModal.value = false
        AppState.activeRecipe = {}
        AppState.ingredients = []
        AppState.steps = []
      }
    })
    return {
      props,
      router,
      favorited,
      showModal,
      authenticated: computed(() => AppState.account.id == props.recipe.creatorId),
      account: computed(() => AppState.account),

      goToDetails() {
        try {
          router.push({ name: 'RecipeDetails', params: { recipeId: props.recipe.id } })

        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
        // await stepsService.getSteps(route.params.recipeId)
      },

      async deleteRecipe() {
        try {
          if (await Pop.confirm()) {
            await recipesService.deleteRecipe(props.recipe.id)
          }
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
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
      }
    }
  }
}
</script>


<style lang="scss" scoped>
@import "src/assets/scss/_variables.scss";
.modalFade-enter-active,
.modalFade-leave-active {
  transition: opacity 0.25s ease;
}

.modalFade-enter-from,
.modalFade-leave-to {
  opacity: 0;
}
.spicebox {
  //NOTE i stole dis from Temani Afif at StackOverflow
  --b: 3px; /* thickness of the border */
  --c: rgb(191, 17, 17); /* color of the border */
  --w: 40px;

  width: 100%;
  height: 60vh;
  box-shadow: 0px 0px 0px 0px var(--c);
  transform: translateY(0px);
  transition: background-color 0.1s ease-out, box-shadow 0.1s ease-out,
    transform 0.2s ease-out;
  border-color: var(--c);
  border: 0px solid transparent; /* space for the border */
  border-bottom: 4px solid var(--c);
  --g: #0000 90deg, var(--c) 0;
  background: conic-gradient(
        from 0deg at bottom var(--b) left var(--b),
        var(--g)
      )
      0 100%,
    conic-gradient(from -90deg at bottom var(--b) right var(--b), var(--g)) 100%
      100%;
  background-size: var(--w) var(--w);
  background-origin: border-box;
  background-repeat: no-repeat;
  background-color: $light;
  &:hover {
    background-color: lighten($light, 10%);
  }
}
.spicebox:hover {
  cursor: pointer;
  box-shadow: 0px 7px 0px 0px var(--c);
  transform: translateY(-4px);
  transition: box-shadow 0.1s ease-in;
  transition: transform 0.1s ease-in;
}

.spicePic {
  height: 35vh;
  width: auto;
  object-fit: cover;
}
.spiceContent {
  height: 25vh;
  width: 100%;
}
hr {
  color: rgb(191, 17, 17);
  height: 2px;
}
</style>