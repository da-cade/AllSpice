<template>
  <div class="myModal d-flex justify-content-center">
    <div class="modalContainer d-flex flex-column bg-light rounded">
      <slot name="modal-content-slot"></slot>
    </div>
  </div>
</template>


<script>
export default {
  setup() {
    return {}
  }
}
</script>


<style lang="scss" scoped>
.myModal {
  position: fixed;
  top: 0;
  right: 0;
  background-color: rgba(0, 0, 0, 0.438);
  height: 100vh;
  width: 100vw;
  z-index: 10;
}
.modalContainer {
  position: relative;
  top: 10vh;
  height: 90vh;
  width: 90vw;
}
</style>