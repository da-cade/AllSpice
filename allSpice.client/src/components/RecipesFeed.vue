<template>
  <div class="row d-flex justify-content-end">
    <div class="col-4">
      <div
        v-if="account.id"
        class="
          filterbar
          d-flex
          justify-content-around
          align-items-center
          bg-dark
          my-5
          rounded
        "
      >
        <div
          class="
            p-2
            d-flex
            align-items-center
            justify-content-center
            selectable
            text-center
            filter
          "
          @click="filter = 'favorited'"
        >
          <h6>Favorited</h6>
        </div>
        <div
          class="
            p-2
            d-flex
            align-items-center
            justify-content-center
            selectable
            text-center
            filter
          "
          @click="filter = 'mine'"
        >
          <h6>My Recipes</h6>
        </div>
        <div
          class="
            p-2
            d-flex
            align-items-center
            justify-content-center
            selectable
            text-center
            filter
          "
          @click="filter = ''"
        >
          <h6>All</h6>
        </div>
      </div>
    </div>
  </div>
  <div class="row">
    <Recipe v-for="r in filteredRecipes" :key="r.id" :recipe="r" />
  </div>
</template>


<script>
import { computed, ref } from "@vue/reactivity"
import { AppState } from "../AppState"
import { watchEffect } from "@vue/runtime-core"
export default {
  setup() {
    const filter = ref('')
    const filteredRecipes = ref([])
    watchEffect(() => {
      let filteredList = AppState.recipes
      if (filter.value && AppState.myFavorites.length) {
        if (filter.value == 'favorited') {
          let favoritesList = AppState.myFavorites
          let arr = []
          favoritesList.forEach(f => {
            // NOTE[COOL] - We manually repopulate the account onto the object because the virtual does not populate two levels deep
            f.recipe.creator = AppState.account
            arr.push(f.recipe)
          })
          filteredList = arr
        }
        if (filter.value == 'mine') {
          filteredList = filteredList.filter(r => r.creatorId == AppState.account.id)
        }
      }
      filteredRecipes.value = filteredList
    })
    return {
      filter,
      filteredRecipes,
      recipes: computed(() => AppState.recipes),
      account: computed(() => AppState.account)
    }
  }
}
</script>


<style lang="scss" scoped>
.filterbar {
  display: flex;
  height: 4vh;
}
.filter {
  min-width: 33%;
  height: 4vh;
}
h6 {
  margin: 0 !important;
}
</style>