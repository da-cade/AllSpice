<template>
  <div>
    <div
      class="d-flex ingredientText"
      v-if="account.id == ingredient.creatorId"
    >
      <input type="text" v-if="editing" v-model.trim.lazy="activeData.name" />
      <span class="w-25" v-else>{{ activeData.name }}</span>

      <input
        type="text"
        v-if="editing"
        v-model.trim.lazy="activeData.quantity"
      />
      <span v-else class="ms-3">{{ activeData.quantity }}</span>
      <div class="on-hover">
        <i
          class="rounded-circle mdi mdi-skull selectable"
          v-if="editing"
          @click="deleteIngredient"
        ></i>
      </div>
    </div>
    <div class="d-flex ingredientText" v-else>
      <span>{{ ingredient.name }}</span>
      <span class="ms-3">{{ ingredient.quantity }}</span>
    </div>
  </div>
</template>


<script>
import { computed, ref } from "@vue/reactivity"
import { AppState } from "../AppState"
import { watchEffect } from "@vue/runtime-core"
import Pop from "../utils/Pop"
import { ingredientsService } from "../services/IngredientsService"
import { logger } from "../utils/Logger"
export default {
  props: {
    ingredient: {
      type: Object,
      required: true
    },
    editing: {
      type: Boolean,
      required: true
    }
  },
  setup(props) {
    const ogData = ref()

    //when the page loads, we capture the original qualities of the ingredient
    ogData.value = { ...props.ingredient }
    const activeData = ref({})
    watchEffect(() => {

      //inside the watch effect, we say if the reactive inputs are empty, fill them with the original data.
      if (!activeData.value.name) {
        console.log('empty')
        activeData.value = { ...ogData.value }
      }
      //next, we check to see if an input, once focus has changed, is different from the original data.
      if ((activeData.value.name != ogData.value.name) || (activeData.value.quantity != ogData.value.quantity)) {

        //if so, we check to see if an edit has already been made to that item.
        let found = AppState.ingEdits.find(ie => ie.id == props.ingredient.id)

        //if either an edit has not been made, or if that edit did not change anything about the original property, we send it to the appstate for editing after
        if (!found || (found.name != activeData.value.name && found.quantity != activeData.value.quantity)) {
          AppState.ingEdits.push(activeData.value)
        }
      }

      // finally, we check the same conditions that the parent checks to run the put request. if they're true, we set the original data value to the new, updated value.
      if (props.editing == false && AppState.ingEdits.length) {
        ogData.value = { ...activeData.value }
      }
      // REVIEW this.$emits.updateMe(endData.value)

    })
    return {
      activeData,
      account: computed(() => AppState.account),
      async deleteIngredient() {
        try {
          if (this.account.id == props.ingredient.creatorId) {
            await ingredientsService.deleteIngredient(props.ingredient.id)
          }
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
input:not(:placeholder-shown):valid {
  border: none;
}
input {
  margin: 0.25rem;
  border: none;
  overflow-y: hidden;
  border-radius: 3px;
}
input:focus {
  outline: none;
}
span {
  margin: 0.15rem;
}
</style>