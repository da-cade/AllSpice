<template>
  <div class="container-fluid g-0">
    <div class="row">
      <div class="col-12">
        <div class=""></div>
      </div>
    </div>
    <div class="row">
      <div class="col-2"></div>
      <div class="col-md-8">
        <RecipesFeed />
      </div>
      <div class="col-1"></div>
      <div class="col-1"></div>
    </div>
  </div>
</template>

<script>
import { onMounted, ref, watchEffect } from "@vue/runtime-core";
import Pop from "../utils/Pop";
import { logger } from "../utils/Logger";
import { recipesService } from "../services/RecipesService";

import { useRoute } from "vue-router";
import { Modal } from "bootstrap";
export default {
  name: 'Home',
  setup() {
    const route = useRoute()
    onMounted(async () => {
      try {
        await recipesService.getAllRecipes()
      } catch (error) {
        logger.error(error)
        Pop.toast(error.message, 'error')
      }
    })
    return {
      route,
    }
  }
}
</script>

<style scoped lang="scss">
.filterbar {
  width: 100%;
  height: 10vh;
}
.spicebox {
  width: 100%;
  height: 40vh;
  background-color: rgb(245, 245, 187);
}
.spicebox:hover {
  cursor: pointer;
}
.spicePic {
  height: 70%;
  width: 100%;
  object-fit: cover;
}
</style>
