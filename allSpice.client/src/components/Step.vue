<template>
  <div>
    <div class="d-flex stepText" v-if="account.id == step.creatorId">
      <input
        type="number"
        style="width: 4vw"
        v-if="editing"
        v-model.trim.lazy="activeData.position"
      />
      <span v-else class="me-3">{{ activeData.position }}.</span>
      <textarea
        :id="'bodyText-' + step.id"
        class="text w-100"
        oninput='this.style.height=""; this.style.height = this.scrollHeight + "px"'
        v-if="editing"
        v-model.trim.lazy="activeData.body"
      />
      <span v-else>{{ activeData.body }}</span>
      <div class="on-hover">
        <i
          class="rounded-circle mdi mdi-skull selectable"
          v-if="editing"
          @click="deleteStep"
        ></i>
      </div>
    </div>
    <div class="d-flex stepText" v-else>
      <span class="me-3">{{ step.position }}.</span>
      <span>{{ step.body }}</span>
    </div>
  </div>
</template>


<script>
import { computed, ref } from "@vue/reactivity"
import { AppState } from "../AppState"
import { onUpdated, watchEffect } from "@vue/runtime-core"
import { logger } from "../utils/Logger"
import Pop from "../utils/Pop"
import { stepsService } from "../services/StepsService"
export default {
  props: {
    step: {
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
    ogData.value = { ...props.step }
    const activeData = ref({})
    watchEffect(() => {
      //inside the watch effect, we say if the reactive inputs are empty, fill them with the original data.
      if (!activeData.value.body) {
        activeData.value = { ...ogData.value }
      }
      //next, we check to see if an input, once focus has changed, is different from the original data.
      if ((activeData.value.body != ogData.value.body) || (activeData.value.position != ogData.value.position)) {

        //if so, we check to see if an edit has already been made to that item.
        let found = AppState.stepEdits.find(se => se.id == props.step.id)

        //finally, if either an edit has not been made, or if that edit did not change anything about the original property, we send it to the appstate for editing after
        if (!found || (found.body != activeData.value.body && found.position != activeData.value.position)) {
          AppState.stepEdits.push(activeData.value)
        }
      }
      // finally, we check the same conditions that the parent checks to run the put request. if they're true, we set the original data value to the new, updated value.
      if (props.editing == false && AppState.stepEdits.length) {
        ogData.value = { ...activeData.value }
      }

    })
    onUpdated(() => {
      if (props.editing) {
        const elem = document.getElementById(`bodyText-${props.step.id}`)
        elem.style.height = elem.scrollHeight + 'px'
      }
    })
    return {
      activeData,
      account: computed(() => AppState.account),
      async deleteStep() {
        try {
          if (this.account.id == props.step.creatorId) {
            await stepsService.deleteStep(props.step.id)
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
textarea:not(:placeholder-shown):valid {
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
textarea {
  border-radius: 3px;
  margin: 0.25rem;
}
input:focus {
  outline: none;
}
textarea:focus {
  outline: none;
}
span {
  margin: 0.15rem;
}
</style>